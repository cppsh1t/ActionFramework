namespace ActioinFramework.HierarchicalTaskNetwork;

[AttributeUsage(AttributeTargets.Class)]
public class TaskNameAttribute(string name) : Attribute {
    public string name = name;
}

public abstract class Task {

    public enum State {
        Running,
        Success,
        Failure
    }

    public State Do(Brain planTimeBrain, Brain realTimeBrain) {
        if (!Predicate(planTimeBrain)) return State.Failure;
        State result = OnDo(planTimeBrain, realTimeBrain);
        Effect(result, planTimeBrain, realTimeBrain);
        return result;
    }

    protected abstract State OnDo(Brain planTimeBrain, Brain realTimeBrain);

    public abstract bool Predicate(Brain planTimeBrain);

    public abstract void Effect(State state, Brain planTimeBrain, Brain realTimeBrain);
}

public abstract class PrimitiveTask : Task {
    protected List<Predicate<Brain>> conditions = [];

    public List<Predicate<Brain>> Conditions => conditions;


    protected List<Func<State, Brain, Brain, bool>> effects = [];

    public List<Func<State, Brain, Brain, bool>> Effects => effects;

    public override bool Predicate(Brain planTimeBrain) {
        for (int i = 0; i < conditions.Count; i++) {
            if (!conditions[i](planTimeBrain)) return false;
        }
        return true;
    }

    public override void Effect(State state, Brain planTimeBrain, Brain realTimeBrain) {
        effects.ForEach(func => func(state, planTimeBrain, realTimeBrain));
    }
}

public class Method {
    public string name = string.Empty;
    public List<Task> tasks = [];
}

public class CompoundTask : Task {
    protected Method? currentMethod;

    //TODO: 列表改用优先队列
    private List<Method> methods = [];
    public void AddMethod(Method method) {
        methods.Add(method);
    }

    public void RemoveMethod(Method method) {
        methods.Remove(method);
    }

    protected override State OnDo(Brain planTimeBrain, Brain realTimeBrain) {
        for (int i = 0; i < currentMethod!.tasks.Count; i++) {
            if (currentMethod!.tasks[i].Do(planTimeBrain, realTimeBrain) == State.Failure) return State.Failure;
        }
        return State.Success;
    }

    public override bool Predicate(Brain planTimeBrain) {
        if (currentMethod != null) {
            for (int i = 0; i < currentMethod.tasks.Count; i++) {
                if (!currentMethod!.tasks[i].Predicate(planTimeBrain)) return false;
            }
            return true;
        }
        else {
            for (int i = 0; i < methods.Count; i++) {
                Method currentLoopMethod = methods[i];
                bool jumpToNext = false;
                for (int y = 0; y < currentLoopMethod.tasks.Count; y++) {
                    if (!currentLoopMethod.tasks[y].Predicate(planTimeBrain)) {
                        jumpToNext = true;
                        break;
                    }
                }
                if (jumpToNext) continue;
                currentMethod = methods[i];
                return true;
            }
            return false;
        }
    }

    public override void Effect(State state, Brain planTimeBrain, Brain realTimeBrain) {
        for (int i = 0; i < currentMethod!.tasks.Count; i++) {
            currentMethod!.tasks[i].Effect(state, planTimeBrain, realTimeBrain);
        }
    }
}