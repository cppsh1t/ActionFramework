namespace ActioinFramework.HierarchicalTaskNetwork;

public class Domain {
    private List<Task> tasks = [];
    private string name = string.Empty;

    public void SetName(string name) {
        this.name = name;
    }

    public void AddTask(Task task) {
        tasks.Add(task);
    }

    public void AddTask(params Task[] tasks) {
        this.tasks.AddRange(tasks);
    }

    public void RemoveTask(Task task) {
        tasks.Remove(task);
    }

    public Task Reslove(Brain brain) {
        return tasks.Find(task => task.Predicate(brain)) ?? throw new InvalidOperationException("无法找到能进行的计划");
    }
}