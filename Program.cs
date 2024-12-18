using ActioinFramework.BehaviourTree.BehaviourNode;
using ActioinFramework.BehaviourTree.Execution;
using ActioinFramework.BehaviourTree.Xml;
using ActioinFramework.GameEngine;
using Task = System.Threading.Tasks.Task;
using ActioinFramework.HierarchicalTaskNetwork;
using HtnTask = ActioinFramework.HierarchicalTaskNetwork.Task;

#pragma warning disable CS8321 
static async Task TestBT() {
    string path = "test.xml";
    NodeXmlDefinition.BuildJsonFile("front/BehaviourTreeBuilder/src/resources/node-definitions.json");
    NodeXmlParser parser = new();
    Node node = parser.Parse(path);

    World world = new(500, 500, TimeSpan.FromSeconds(60));
    // SequenceNode sequenceNode = new();
    // sequenceNode.AddNode(new WaitNode(2));
    // sequenceNode.AddNode(new HealNode());
    // RepeatNode repeatNode = new();
    // repeatNode.SetChild(sequenceNode);
    BehaviourTreeEntity entity = new("heal-man", node) {
        Health = 50
    };
    world.AddEntity(entity);
    await world.Log();
}


static async Task TestHTN() {
    World world = new(500, 500, TimeSpan.FromSeconds(60));
    Domain domain = new();
    domain.AddTask(new SleepTask(), new DefaultTask());
    Planner planner = new(new Brain(), domain);
    PlanRunner planRunner = new("miner", planner);
    world.AddEntity(planRunner);
    await world.Log();
}

await TestHTN();

#pragma warning restore CS8321

class SleepTask : HtnTask {
    public override void Effect(State state, Brain planTimeBrain, Brain realTimeBrain) {
        var host = (IEntity)realTimeBrain.Reslove("host") ?? throw new InvalidCastException("error");
        if (state == State.Running) {
            host.Health++;
            object sleepTime = realTimeBrain.Reslove("sleepTime") ?? 0;
            int castedSleepTime = (int)sleepTime;
            realTimeBrain.Put("sleepTime", ++castedSleepTime);
        }
        else if (state == State.Success) {
            // realTimeBrain.Put("sleepTime", 0);
        }

    }

    public override bool Predicate(Brain planTimeBrain) {
        object sleepTime = planTimeBrain.Reslove("sleepTime") ?? 0;
        int castedSleepTime = (int)sleepTime;
        return castedSleepTime <= 10;
    }

    protected override State OnDo(Brain planTimeBrain, Brain realTimeBrain) {
        var host = (IEntity)realTimeBrain.Reslove("host") ?? throw new InvalidCastException("error");
        object sleepTime = realTimeBrain.Reslove("sleepTime") ?? 0;
        int castedSleepTime = (int)sleepTime;

        if (castedSleepTime > 10) {
            host.Log("睡够了");
            return State.Success;
        }

        host.Log($"睡觉中....  睡觉值:{castedSleepTime}");
        return State.Running;
    }
}

class DefaultTask : HtnTask {
    public override void Effect(State state, Brain planTimeBrain, Brain realTimeBrain) {

    }

    public override bool Predicate(Brain planTimeBrain) {
        return true;
    }

    protected override State OnDo(Brain planTimeBrain, Brain realTimeBrain) {
        var host = (IEntity)realTimeBrain.Reslove("host") ?? throw new InvalidCastException("error");
        host.Log("目标无法活动");
        return State.Running;
    }
}