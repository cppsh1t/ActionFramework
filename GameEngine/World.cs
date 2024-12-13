namespace ActioinFramework.GameEngine;

public interface IEntity {
    public Transform Transform { get; set; }
    public int Health { get; set; }
    public string Name { get; }
    public void Start();
    public void Update();
    public void FixedUpdate();
    public void LogInfo() {
        Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss:fff")}] [{Name} info]: [position: {{ x: {Transform.Position.X}, y: {Transform.Position.Y} }}], [hp: {Health}]");
    }
}

public abstract class Entity(string name) : IEntity {
    public Transform Transform { get; set; } = new Transform();

    public int Health { get; set; } = 100;
    public string Name { get; } = name;

    public abstract void FixedUpdate();

    public abstract void Start();

    public abstract void Update();
}

public class Transform {
    public Vector Position { get; private set; } = new Vector(0, 0);
}

public class World {
    private readonly int updateTick;
    private readonly int fixedUpdateTick;
    private readonly TimeSpan overTime;

    public IList<IEntity> Entities { get; private set; } = [];

    public static World? Instance { get; private set; }

    public World(int updateTick, int fixedUpdateTick, TimeSpan overTime) {
        this.updateTick = updateTick;
        this.fixedUpdateTick = fixedUpdateTick;
        this.overTime = overTime;
        Instance = this;
    }

    public void AddEntity(IEntity entity) {
        Entities.Add(entity);
    }

    public async Task Log() {
        foreach (var entity in Entities) {
            entity.Start();
        }

        Task updateTask = Task.Run(async () => {
            while (true) {
                foreach (var entity in Entities) {
                    entity.Update();
                    entity.LogInfo();
                }
                await Task.Delay(updateTick);
            }
        });

        Task fixedUpdateTask = Task.Run(async () => {
            while (true) {
                foreach (var entity in Entities) {
                    entity.FixedUpdate();
                }
                await Task.Delay(fixedUpdateTick);
            }
        });

        await Task.WhenAny(updateTask, fixedUpdateTask, Task.Delay(overTime));
    }
}