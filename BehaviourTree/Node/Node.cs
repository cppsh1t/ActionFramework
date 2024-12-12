namespace ActioinFramework.BehaviourTree.BehaviourNode;

public abstract class Node
{
    public enum State
    {
        Wait,
        Running,
        Failure,
        Success
    }

    public enum NodeType
    {
        Action,
        Composite,
        Decorator,
    }

    public State CurrentState { get; private set; } = State.Wait;

    public bool Started => CurrentState != State.Wait;

    public abstract void OnStart();
    public abstract State OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnStop();
}

public abstract class ActionNode : Node
{
    public readonly NodeType Type = NodeType.Action;
}

public abstract class CompositeNode : Node
{
    public readonly NodeType Type = NodeType.Composite;
    public IList<Node> Nodes { get; protected set; } = [];

    public Action<Node> OnAdd { get; protected set; } = (node) => { };
    public Action<Node> OnRemove { get; protected set; } = (node) => { };

    public virtual void AddNode(Node node)
    {
        Nodes.Add(node);
        OnAdd(node);
    }

    public virtual void RemoveNode(Node node)
    {
        Nodes.Remove(node);
        OnRemove(node);
    }
}

public abstract class DecoratorNode : Node
{
    public readonly NodeType Type = NodeType.Decorator;
    public Node? Child { get; protected set; } = null;

    public void SetChild(Node node)
    {
        Child = node;
    }

    public void ClearChild()
    {
        Child = null;
    }

}