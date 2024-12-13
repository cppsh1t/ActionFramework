using ActioinFramework.BehaviourTree.BehaviourNode;
using ActioinFramework.BehaviourTree.Execution;
using ActioinFramework.GameEngine;

World world = new(500, 500, TimeSpan.FromSeconds(20));
BehaviourTreeEntity entity = new("heal-man", new HealNode());
world.AddEntity(entity);
await world.Log();

[NodeXmlName("heal")]
[NodeXmlProperty("heal", "healValue"), NodeXmlProperty("stop", "stopValue")]
class HealNode : ActionNode {
    private int healValue = 5;
    private int stopValue = 100;

    public override void OnStart() {
        host!.Health = 50;
    }

    public override void OnStop() {

    }

    protected override void OnFixedUpdate() {

    }

    protected override State OnUpdate() {
        host!.Health += healValue;
        Console.WriteLine($"{host.Name} 恢复了 {healValue}点血量");
        if (host!.Health >= stopValue) {
            Console.WriteLine($"血量已恢复到{stopValue}及以上, 停止");
            return State.Success;
        }
        return State.Running;
    }
}