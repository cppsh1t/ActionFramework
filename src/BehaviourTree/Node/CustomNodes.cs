namespace ActioinFramework.BehaviourTree.BehaviourNode;

[NodeXmlName("heal")]
[NodeXmlProperty("heal", "healValue"), NodeXmlProperty("stop", "stopValue")]
public class HealNode : ActionNode {
    private int healValue = 5;
    private int stopValue = 100;

    public override void OnStart() {
    }

    public override void OnStop() {

    }

    protected override void OnFixedUpdate() {

    }

    protected override State OnUpdate() {
        if (host!.Health >= stopValue) {
            host?.Log("无需治疗");
            return State.Success;
        }
        host!.Health += healValue;
        host?.Log($"{host.Name} 恢复了 {healValue}点血量");
        if (host!.Health >= stopValue) {
            host?.Log($"血量已恢复到{stopValue}及以上, 停止");
            return State.Success;
        }
        return State.Running;
    }
}