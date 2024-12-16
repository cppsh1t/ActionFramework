using ActioinFramework.GameEngine;

namespace ActioinFramework.BehaviourTree.BehaviourNode;

public readonly struct XmlProperty(string attributeName, string fieldName) {
    public readonly string attributeName = attributeName;
    public readonly string fieldName = fieldName;
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class NodeXmlNameAttribute(string name) : Attribute {
    public readonly string name = name;
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class NodeXmlPropertyAttribute(string attributeName, string fieldName) : Attribute {
    public readonly string attributeName = attributeName;
    public readonly string fieldName = fieldName;
}

public abstract class Node : INodeBehaviour {
    public enum State {
        Wait,
        Running,
        Failure,
        Success
    }

    public enum NodeType {
        Action,
        Composite,
        Decorator,
    }

    protected IEntity? host;

    public State CurrentState { get; protected set; } = State.Wait;

    public bool Started => CurrentState != State.Wait;

    public abstract void OnStart();
    protected abstract State OnUpdate();
    protected abstract void OnFixedUpdate();
    public abstract void OnStop();

    public abstract State Update();

    public abstract void FixedUpdate();

    public virtual void Init(IEntity host) {
        this.host = host;
    }

}

public interface INodeBehaviour {
    public Node.State Update();
    public void FixedUpdate();
}


public abstract class ActionNode : Node {
    public readonly NodeType Type = NodeType.Action;

    public sealed override State Update() {
        if (CurrentState == State.Failure || CurrentState == State.Success) return CurrentState;
        CurrentState = OnUpdate();
        if (CurrentState == State.Failure || CurrentState == State.Success) {
            OnStop();
        }
        return CurrentState;
    }

    public sealed override void FixedUpdate() {
        OnFixedUpdate();
    }
}

public abstract class CompositeNode : Node {
    public readonly NodeType Type = NodeType.Composite;
    public List<Node> Nodes { get; protected set; } = [];

    public Action<Node> OnAdd { get; protected set; } = (node) => { };
    public Action<Node> OnRemove { get; protected set; } = (node) => { };

    public virtual void AddNode(Node node) {
        Nodes.Add(node);
        OnAdd(node);
    }

    public virtual void RemoveNode(Node node) {
        Nodes.Remove(node);
        OnRemove(node);
    }

    public override void Init(IEntity host) {
        base.Init(host);
        Nodes.ForEach(item => item.Init(host));
    }
}

public abstract class DecoratorNode : Node {
    public readonly NodeType Type = NodeType.Decorator;
    public Node? Child { get; protected set; } = null;

    public void SetChild(Node node) {
        Child = node;
    }

    public void ClearChild() {
        Child = null;
    }

    public override void OnStart() {
        Child?.OnStart();
    }

    public sealed override State Update() {
        CurrentState = OnUpdate();
        if (CurrentState == State.Failure || CurrentState == State.Success) {
            OnStop();
        }
        return CurrentState;
    }

    public sealed override void FixedUpdate() {
        OnFixedUpdate();
    }

    public override void Init(IEntity host) {
        base.Init(host);
        Child?.Init(host);
    }
}