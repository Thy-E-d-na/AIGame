
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class BehaviourTree : MonoBehaviour
{
    public delegate void DoAHandler();

    public DoAHandler doaChanged;
   
    void enemyDead()
    {
        doaChanged?.Invoke();
    }

    public NavMeshAgent agent;
    public Transform player;

    public float currentHealth = 100f;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float patrolRadius = 8f;

    public Selector selector;
    public Sequence sequence;
    private Vector3 startPosition;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;

        //create BT
        //1. Patrol
        Patrol patrol = new Patrol(agent, startPosition, patrolRadius);
        patrol.order = 2;
        //2.Chase
        Sequence chase = new Sequence(new List<Node> { new CheckDistance(transform, player, chaseRange),new Chase(agent,player)});
        chase.order = 1;
        //3. Atk
        Sequence atk = new Sequence(new List<Node> { new CheckDistance(transform,player, attackRange), new Attack(agent,transform,player)});
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
        if (currentHealth <= 0) enemyDead();
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

public class Selector: Node
{
    protected List<Node> nodes = new();
    public Selector(List<Node> nodes)
    {
        this.nodes = nodes;
        SortChildren();
    }
    public void SortChildren()
    {
        nodes.Sort((a,b) => a.order.CompareTo(b.order));
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

public class Sequence: Node
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
public class Patrol: Node
{
    private NavMeshAgent e_agent;
    private Vector3 startPos;
    private float patrolR;

    public Patrol(NavMeshAgent _agent,Vector3 _startPos, float _r)
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
public class Chase: Node
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
public class Attack: Node
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
        return state;
    }

}

#endregion


#region CHECK CONDITION LEAF

public class CheckDistance: Node
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

#endregion