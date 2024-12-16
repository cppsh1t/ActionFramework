namespace ActioinFramework.BehaviourTree.BehaviourNode;

[NodeXmlName("sequence")]
[NodeXmlProperty("index", "currentIndex")]
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
        State currentState = OnUpdate();

        if (currentState == State.Failure) return State.Failure;
        if (currentState == State.Success) {
            CurrentNode.OnStop();
            if (currentIndex + 1 >= Nodes.Count) {
                return State.Success;
            }
            currentIndex++;
            CurrentNode.OnStart();
        }

        return State.Running;
    }
}

[NodeXmlName("wait")]
[NodeXmlProperty("wait", "waitSecond")]
public class WaitNode(int waitSecond) : ActionNode {

    private DateTime startTime;
    private int waitSecond = waitSecond;

    public WaitNode() : this(0) {

    }

    public override void OnStart() {
        startTime = DateTime.Now;
    }

    public override void OnStop() {

    }

    protected override void OnFixedUpdate() {

    }

    protected override State OnUpdate() {
        if (startTime.AddSeconds(waitSecond) <= DateTime.Now) {
            host?.Log($"等待计时到达, {DateTime.Now}");
            return State.Success;
        }
        else {
            host?.Log("等待中...");
            return State.Running;
        }
    }
}

public class RepeatNode : DecoratorNode {

    private bool stopWhenFailure = false;

    public override void OnStop() {

    }

    protected override void OnFixedUpdate() {

    }

    protected override State OnUpdate() {
        var state = Child?.Update();
        if (state == State.Failure && stopWhenFailure)
            return State.Failure;
        return State.Running;
    }
}