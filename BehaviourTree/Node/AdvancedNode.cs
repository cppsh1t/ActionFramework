namespace ActioinFramework.BehaviourTree.BehaviourNode;

public class SequenceNode : CompositeNode {
    public int CurrentIndex => currentIndex;
    private int currentIndex = 0;
    public Node CurrentNode => Nodes[CurrentIndex];

    public sealed override void FixedUpdate() {
        OnFixedUpdate();
    }

    protected override void OnFixedUpdate() {
        CurrentNode.FixedUpdate();
    }

    public override void OnStart() {
        CurrentNode.OnStart();
    }

    public override void OnStop() {
        currentIndex = 0;
    }

    protected override State OnUpdate() {
        return CurrentNode.Update();
    }

    public sealed override State Update() {
        if (CurrentIndex >= Nodes.Count) {
            return State.Success;
        }
        State currentState = OnUpdate();

        if (currentState == State.Failure) return State.Failure;
        if (currentState == State.Success) {
            CurrentNode.OnStop();
            currentIndex++;
            CurrentNode.OnStart();
        }

        return State.Running;
    }
}