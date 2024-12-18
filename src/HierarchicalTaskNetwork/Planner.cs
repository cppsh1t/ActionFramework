using ActioinFramework.GameEngine;

namespace ActioinFramework.HierarchicalTaskNetwork;

public class Planner(Brain brain, Domain domain) {
    private Brain realTimeBrain = brain;
    public Brain RealTimeBrain => realTimeBrain;
    private Domain domain = domain;
    private Brain? planTimeBrain;

    public Task Plan() {
        planTimeBrain = realTimeBrain.Clone();
        return domain.Reslove(planTimeBrain);
    }

    public Task.State DoPlan(Task task) {
        return task.Do(planTimeBrain!, realTimeBrain);
    }
}

public class PlanRunner : Entity {

    private readonly Planner planner;
    private Task? currentTask = null;

    public PlanRunner(string name, Planner planner) : base(name) {
        this.planner = planner;
        planner.RealTimeBrain.Put("host", this);
    }

    public override void FixedUpdate() {

    }

    public override void Start() {
        currentTask = planner.Plan();
    }

    public override void Update() {
        if (planner.DoPlan(currentTask!) != Task.State.Running) {
            currentTask = planner.Plan();
        }
    }
}