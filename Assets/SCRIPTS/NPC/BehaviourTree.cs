
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class BehaviourTree : MonoBehaviour
{
    // public delegate void DoAHandler();

    //public DoAHandler doaChanged;

    //void enemyDead()
    //{
    //    doaChanged?.Invoke();
    //}
    public float normalVision = 10f;
    public float normalFoV = 90f;
    public float stealthVision = 4f;
    public float stealthFoV = 70f;

    [Header("normal vision")]

    public float visualRange; // vision range
    public float viewAngle; // fov
    private Light lightVision;

    [Header("obstacles")]
    public LayerMask obstacleMask;

    // delay time variable
    public float sensorTickRate = 0.5f;

    void HandleStealthState(bool isStealth)
    {
        if (isStealth)
        {
            visualRange = stealthVision;
            viewAngle = stealthFoV;

        }
        else
        {
            visualRange = normalVision;
            viewAngle = normalFoV;

        }
        UpdateVisionLight();
    }

    void UpdateVisionLight()
    {
        if (lightVision == null) return;
        lightVision.range = visualRange;
        lightVision.spotAngle = viewAngle;
    }

    public bool playerDetect;

    public NavMeshAgent agent;
    public Transform playerPos;

    public float currentHealth = 100f;
    public float chaseRange = 8f;
    public float attackRange = 3f;
    public float patrolRadius = 8f;

    public Selector selector;
    public Sequence sequence;
    private Vector3 startPosition;
    private void Start()
    {

        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        player.OnStealthChanged += HandleStealthState;

        HandleStealthState(false);
        lightVision = GetComponentInChildren<Light>();
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;

        //create BT
        //1. Patrol
        Patrol patrol = new Patrol(agent, startPosition, patrolRadius);
        patrol.order = 2;
        //2.Chase
        Sequence chase = new Sequence(new List<Node> { new enemyVision(this, transform, playerPos), new CheckDistance(transform, playerPos, chaseRange),new Chase(agent, playerPos) });
        chase.order = 1;
        //3. Atk
        Sequence atk = new Sequence(new List<Node> { new CheckDistance(transform, playerPos, attackRange), new Attack(agent, transform, playerPos) });
        atk.order = 0;
        selector = new Selector(new List<Node>
        {
            patrol,
            chase,
            atk,
        });
    }

    private void Update()
    {
        //if (currentHealth <= 0) enemyDead();
        selector.Evaluate();
    }
}


#region BT - abstract

public enum NodeState
{
    Running,
    Success,
    Failed,
}

public abstract class Node
{
    public int order = 0;
    protected NodeState state;
    public NodeState nodeState { get { return state; } }
    public abstract NodeState Evaluate();
}

#endregion


#region   COMPOSITE

public class Selector : Node
{
    protected List<Node> nodes = new();
    public Selector(List<Node> nodes)
    {
        this.nodes = nodes;
        SortChildren();
    }
    public void SortChildren()
    {
        nodes.Sort((a, b) => a.order.CompareTo(b.order));
    }
    public override NodeState Evaluate()
    {
        foreach (Node node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.Failed:
                    continue;

                case NodeState.Success:
                    state = NodeState.Success;
                    return state;

                case NodeState.Running:
                    state = NodeState.Running;
                    return state;
                default: continue;
            }
        }
        state = NodeState.Failed;
        return state;
    }
}

public class Sequence : Node
{
    protected List<Node> nodes = new();

    public Sequence(List<Node> nodes)
    {
        this.nodes = nodes;
        SortChildren();
    }

    public void SortChildren()
    {
        nodes.Sort((a, b) => a.order.CompareTo(b.order));
    }
    public override NodeState Evaluate()
    {
        var anyChildRunning = false;

        foreach (Node n in nodes)
        {
            switch (n.Evaluate())
            {
                case NodeState.Failed:
                    state = NodeState.Failed;
                    return state;
                case NodeState.Success:
                    continue;
                case NodeState.Running:
                    anyChildRunning = true;
                    continue;
                default:
                    state = NodeState.Success;
                    return state;
            }
        }
        state = anyChildRunning ? NodeState.Running : NodeState.Success;
        return state;
    }
}

#endregion


#region ACTION LEAF

//patroling
public class Patrol : Node
{
    private NavMeshAgent e_agent;
    private Vector3 startPos;
    private float patrolR;

    public Patrol(NavMeshAgent _agent, Vector3 _startPos, float _r)
    {
        this.e_agent = _agent;
        this.startPos = _startPos;
        this.patrolR = _r;
    }
    public override NodeState Evaluate()
    {
        e_agent.isStopped = false;
        e_agent.speed = 5f;
        if (!e_agent.pathPending && e_agent.remainingDistance <= 2)
        {
            var randomDirection = Random.insideUnitSphere * patrolR;
            randomDirection += startPos;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, patrolR, NavMesh.AllAreas))
            {
                e_agent.SetDestination(hit.position);

            }
        }
        state = NodeState.Running;
        return state;
    }
}

//chase
public class Chase : Node
{
    private NavMeshAgent e_agent;
    private Transform _target;

    public Chase(NavMeshAgent agent, Transform target)
    {
        this.e_agent = agent;
        this._target = target;
    }

    public override NodeState Evaluate()
    {
        e_agent.isStopped = false;
        e_agent.speed = 10f;
        e_agent.SetDestination(_target.position);
        state = NodeState.Running;
        return state;
    }
}
//attack
public class Attack : Node
{
    private NavMeshAgent e_agent;
    private Transform _target;
    private Transform _transform;

    public Attack(NavMeshAgent agent, Transform transform, Transform target)
    {
        this.e_agent = agent;
        this._transform = transform;
        this._target = target;
    }
    public override NodeState Evaluate()
    {
        e_agent.isStopped = true; // dung lai de danh
        _transform.LookAt(_target);        
        state = NodeState.Running;
        soundMngt.sInstance.PlaySfx(1);
        gameMngt.Instance.isDefeated = true;
        return state;
    }

}

#endregion


#region CHECK CONDITION LEAF

public class CheckDistance : Node
{
    private Transform _transform;
    private Transform _target;
    private float _range;

    public CheckDistance(Transform transform, Transform target, float range)
    {
        this._transform = transform;
        this._target = target;
        this._range = range;
    }
    public override NodeState Evaluate()
    {
        var distance = Vector3.Distance(_transform.position, _target.position);
        return distance <= this._range ? NodeState.Success : NodeState.Failed;
    }
}
public class enemyVision : Node
{
    private BehaviourTree _npc;
    private Transform _target;
    private Transform _transform;
    public enemyVision(BehaviourTree npc, Transform transform, Transform target)
    {
        this._npc = npc;
        this._target = target;
        this._transform = transform;
    }
    public override NodeState Evaluate()
    {
        _npc.playerDetect = false;
        var distance = Vector3.Distance(_transform.position, _target.position);
        if (distance <= _npc.visualRange)
        {
            var direction = (_target.position - _transform.position).normalized;
            var angle = Vector3.Angle(_transform.forward, direction);
            if (angle < _npc.viewAngle / 2)
            {
                if (!Physics.Raycast(_transform.position, direction, out RaycastHit hit, distance, _npc.obstacleMask))
                    _npc.playerDetect = true;
                soundMngt.sInstance.PlaySfx(2);
            }
        }
        return _npc.playerDetect ? NodeState.Success : NodeState.Failed;

    }
}

#endregion