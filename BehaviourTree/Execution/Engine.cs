using ActioinFramework.BehaviourTree.BehaviourNode;

namespace ActioinFramework.BehaviourTree.Execution;

public abstract class Engine
{
    private Node rootNode;
    public Node CurrentNode { get; private set; }

    public Engine(Node rootNode)
    {
        this.rootNode = rootNode;
        CurrentNode = rootNode;
    }

    public void SetRootNode(Node rootNode)
    {
        this.rootNode = rootNode;
    }

    public void Start()
    {
        CurrentNode.OnStart();
    }

    public void Update()
    {
        CurrentNode.OnUpdate();
    }

}