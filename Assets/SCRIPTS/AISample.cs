//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//// trang thai cua 1 node
//public enum NodeState
//{
//    Running, Success, Failure
//}

//public abstract class Node
//{
//    public int order = 0; // trọng số độ ưu tiên
//    protected NodeState state;
//    public NodeState nodeState { get { return state; } }
//    public abstract NodeState Evaluate();
//}

//public class Selector : Node
//{
//    protected List<Node> nodes = new();

//public Selector(List<Node> nodes)
//{
//    this.nodes = nodes;
//    SortChildren();
//}

//    public void SortChildren()
//    {
//        nodes.Sort((a, b) => a.order.CompareTo(b.order));
//    }

//    public override NodeState Evaluate()
//    {
//        foreach (Node n in nodes)
//        {
//            switch (n.Evaluate())
//            {
//                case NodeState.Failure:
//                    continue;

//                case NodeState.Success:
//                    state = NodeState.Success;
//                    return state;

//                case NodeState.Running:
//                    state = NodeState.Running;
//                    return state;

//                default: continue;
//            }
//        }
//        state = NodeState.Failure;
//        return state;
//    }
//}

//public class Sequence : Node
//{
//    protected List<Node> nodes = new();

//    public Sequence(List<Node> nodes)
//    {
//        this.nodes = nodes;
//        SortChildren();
//    }

//    public void SortChildren()
//    {
//        nodes.Sort((a, b) => a.order.CompareTo(b.order));
//    }

//    public override NodeState Evaluate()
//    {
//        var anyChildRunning = false;
//        foreach (Node n in nodes)
//        {
//            switch (n.Evaluate())
//            {
//                case NodeState.Failure:
//                    state = NodeState.Failure;
//                    return state;
//                case NodeState.Success:
//                    continue;
//                case NodeState.Running:
//                    anyChildRunning = true;
//                    continue;
//                default:
//                    state = NodeState.Success;
//                    return state;
//            }
//        }
//        state = anyChildRunning ? NodeState.Running : NodeState.Success;
//        return state;
//    }
//}

//public abstract class Decorator : Node
//{
//    protected Node child;
//    public Decorator(Node child)
//    {
//        this.child = child;
//    }
//}

//// cooldown giới hạn thời gian
//public class CooldownDecorator : Decorator
//{
//    private float _cooldownTime;
//    private float _lastCooldownTime;

//    public CooldownDecorator(Node child, float cooldownTime) : base(child)
//    {
//        this._cooldownTime = cooldownTime;
//    }

//    public override NodeState Evaluate()
//    {
//        if (Time.time - _lastCooldownTime > _cooldownTime)
//        {
//            state = child.Evaluate();
//            if (state == NodeState.Success || state == NodeState.Running)
//            {
//                _lastCooldownTime = Time.time;
//            }
//            return state;
//        }

//        state = NodeState.Failure;
//        return state;
//    }
//}


//// quái: đi tuần, duoi theo, tan cong, chay tron
//public class CheckHealthNode : Node
//{
//    private NpcAI npc;
//    private float threshold;

//    public CheckHealthNode(NpcAI npc, float threshold)
//    {
//        this.npc = npc;
//        this.threshold = threshold;
//    }

//    public override NodeState Evaluate()
//    {
//        return npc.currentHealth <= threshold ? NodeState.Success : NodeState.Failure;
//    }
//}

//public class CheckDistanceNode : Node
//{
//    private Transform _transform;
//    private Transform _target;
//    private float _range;

//    public CheckDistanceNode(Transform transform, Transform target, float range)
//    {
//        this._transform = transform;
//        this._target = target;
//        this._range = range;
//    }
//    public override NodeState Evaluate()
//    {
//        var distance = Vector3.Distance(_transform.position, _target.position);
//        return distance <= this._range ? NodeState.Success : NodeState.Failure;
//    }
//}

//public class TaskFleeNode : Node
//{
//    private NavMeshAgent _agent;
//    private Transform _transform;
//    private Transform _target;
//    private float _fleeDistance;

//    public TaskFleeNode(NavMeshAgent agent, Transform transform,
//        Transform target, float fleeDistance)
//    {
//        this._agent = agent;
//        this._transform = transform;
//        this._target = target;
//        this._fleeDistance = fleeDistance;
//    }
//    public override NodeState Evaluate()
//    {
//        var fleeDirection = (_transform.position - _target.position).normalized;

//        var fleeTarget = _transform.position + fleeDirection * this._fleeDistance;
//        NavMeshHit hit;
//        if (NavMesh.SamplePosition(fleeTarget, out hit, 5f, NavMesh.AllAreas))
//        {
//            _agent.SetDestination(hit.position);
//        }

//        _agent.isStopped = false;
//        _agent.speed = 10f;
//        Debug.Log("Chạy nhanh ......");
//        state = NodeState.Running;
//        return state;
//    }
//}

//public class TaskAttackNode : Node
//{
//    private NavMeshAgent _agent;
//    private Transform _target;
//    private Transform _transform;

//    public TaskAttackNode(NavMeshAgent agent, Transform transform, Transform target)
//    {
//        this._agent = agent;
//        this._transform = transform;
//        this._target = target;
//    }
//    public override NodeState Evaluate()
//    {
//        _agent.isStopped = true; // dung lai de danh
//        _transform.LookAt(_target);
//        Debug.Log("....Dang chem Player....");
//        state = NodeState.Running;
//        return state;
//    }
//}

//public class TaskChaseNode : Node
//{
//    private NavMeshAgent _agent;
//    private Transform _target;

//    public TaskChaseNode(NavMeshAgent agent, Transform target)
//    {
//        this._agent = agent;
//        this._target = target;
//    }

//    public override NodeState Evaluate()
//    {
//        _agent.isStopped = false;
//        _agent.speed = 10f;
//        _agent.SetDestination(_target.position);
//        Debug.Log("...Dang duoi theo Player....");
//        state = NodeState.Running;
//        return state;
//    }
//}

//public class TaskPatrolNode : Node
//{
//    private NavMeshAgent _agent;
//    private Vector3 startPosition;
//    private float patrolRadius;

//    public TaskPatrolNode(NavMeshAgent agent, Vector3 startPosition, float patrolRadius)
//    {
//        this._agent = agent;
//        this.startPosition = startPosition;
//        this.patrolRadius = patrolRadius;
//    }

//    public override NodeState Evaluate()
//    {
//        _agent.isStopped = false;
//        _agent.speed = 5f;

//        if (!_agent.pathPending && _agent.remainingDistance <= 2)
//        {
//            var randomDirection = Random.insideUnitSphere * patrolRadius;
//            randomDirection += startPosition;
//            NavMeshHit hit;
//            if (NavMesh.SamplePosition(randomDirection, out hit,
//                    patrolRadius, NavMesh.AllAreas))
//            {
//                _agent.SetDestination(hit.position);
//            }
//        }
//        Debug.Log("...Dang di tuan tra....");
//        state = NodeState.Running;
//        return state;
//    }
//}


//public class NpcAI : MonoBehaviour
//{
//    public NavMeshAgent agent;
//    public Transform player;

//    public float currentHealth = 100f;
//    public float fleeHealth = 30f;
//    public float chaseRange = 10f;
//    public float attackRange = 2f;
//    public float patrolRadius = 8f;

//    public Selector rootNode;
//    public Sequence fleeSequence;
//    private bool isMutated = false; // có thay đổi không?

//    private Vector3 startPosition;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        startPosition = transform.position;

//        // tạo cây BT
//        // 1. nhánh bỏ chạy (order = 10)
//        fleeSequence = new Sequence(new List<Node>{new CheckHealthNode(this, fleeHealth),new TaskFleeNode(agent, transform, player, 10f)});
//        fleeSequence.order = 10;
//        // 2. nhanh tan cong
//        Sequence attackSequence = new Sequence(new List<Node>{    new CheckDistanceNode(transform, player, attackRange),new CooldownDecorator(new TaskAttackNode(agent, transform, player), 2f)});
//        attackSequence.order = 1;
//        // 3. nhanh duoi theo
//        Sequence chaseSequence = new Sequence(new List<Node>{ new CheckDistanceNode(transform, player, chaseRange),new TaskChaseNode(agent, player)});
//        chaseSequence.order = 2;
//        // 4. nhanh di tuan
//        TaskPatrolNode patrolNode = new TaskPatrolNode(agent, startPosition, patrolRadius);
//        patrolNode.order = 3;
//        // cay
//        rootNode = new Selector(new List<Node>
//        {
//            fleeSequence, // 10, 0
//            attackSequence, // 1
//            chaseSequence, // 2
//            patrolNode // 3
//        });
//    }

//    void Update()
//    {
//        // kiem tra dot bien
//        if (currentHealth <= fleeHealth && !isMutated)
//        {
//            Debug.Log("...Het mau, chay tron....");
//            fleeSequence.order = 0;
//            rootNode.SortChildren();
//            isMutated = true;
//        }

//        rootNode.Evaluate();
//    }
//}
