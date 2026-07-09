
using System.Collections.Generic;
using UnityEngine;



public class BehaviourTree : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


#region BT - abstract

public enum NodeState
{
    Running,
    Succcess,
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

                case NodeState.Succcess:
                    state = NodeState.Succcess;
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
                case NodeState.Succcess:
                    continue;
                case NodeState.Running:
                    anyChildRunning = true;
                    continue;
                default:
                    state = NodeState.Succcess;
                    return state;
            }
        }
        state = anyChildRunning ? NodeState.Running : NodeState.Succcess;
        return state;
    }
}

#endregion


#region ACTION LEAF

//patroling

//attack

//decompose

#endregion


#region CHECK CONDITION LEAF

#endregion