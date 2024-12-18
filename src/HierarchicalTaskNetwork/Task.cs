namespace ActioinFramework.HierarchicalTaskNetwork;

[AttributeUsage(AttributeTargets.Class)]
public class TaskNameAttribute(string name) : Attribute {
    public string name = name;
}

public abstract class Task {
#pragma warning disable CS8618 
    protected Brain brain;
#pragma warning restore CS8618 

    public virtual void Init(Brain brain) {
        this.brain = brain;
    }

    public bool Do() {
        if (!Predicate()) return false;
        if (!OnDo()) return false;
        Effect();
        return true;
    }

    protected abstract bool OnDo();

    public abstract bool Predicate();

    public abstract void Effect();
}

public abstract class PrimitiveTask : Task {
    protected List<Predicate<Brain>> conditions = [];

    public List<Predicate<Brain>> Conditions => conditions;


    protected List<Predicate<Brain>> effects = [];

    public List<Predicate<Brain>> Effects => effects;

    public override bool Predicate() {
        for (int i = 0; i < conditions.Count; i++) {
            if (!conditions[i](brain)) return false;
        }
        return true;
    }

    public override void Effect() {
        effects.ForEach(func => func(brain));
    }
}

public class Method {
    public string name = string.Empty;
    public List<Task> tasks = [];
}

public class CompoundTask : Task {
    protected Method? currentMethod;
    private List<Method> methods = [];
    public void AddMethod(Method method) {
        methods.Add(method);
    }

    public void RemoveMethod(Method method) {
        methods.Remove(method);
    }

    protected override bool OnDo() {
        for (int i = 0; i < currentMethod!.tasks.Count; i++) {
            if (!currentMethod!.tasks[i].Do()) return false;
        }
        return true;
    }

    public override bool Predicate() {
        if (currentMethod != null) {
            for (int i = 0; i < currentMethod.tasks.Count; i++) {
                if (!currentMethod!.tasks[i].Predicate()) return false;
            }
            return true;
        }
        else {
            for (int i = 0; i < methods.Count; i++) {
                Method currentLoopMethod = methods[i];
                bool jumpToNext = false;
                for (int y = 0; y < currentLoopMethod.tasks.Count; y++) {
                    if (!currentLoopMethod.tasks[y].Predicate()) {
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

    public override void Effect() {
        for (int i = 0; i < currentMethod!.tasks.Count; i++) {
            currentMethod!.tasks[i].Effect();
        }
    }
}