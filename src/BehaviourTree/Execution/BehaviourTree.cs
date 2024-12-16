using ActioinFramework.BehaviourTree.BehaviourNode;
using ActioinFramework.GameEngine;

namespace ActioinFramework.BehaviourTree.Execution;

public class BehaviourTree(Node rootNode) {
    private Node rootNode = rootNode;

    public void SetRootNode(Node rootNode) {
        this.rootNode = rootNode;
    }

    public void Start() {
        rootNode.OnStart();
    }

    public void Update() {
        rootNode.Update();
    }

    public void FixedUpdate() {
        rootNode.FixedUpdate();
    }

    public void Init(Entity host) {
        rootNode.Init(host);
    }
}

public class BehaviourTreeEntity : Entity {
    private readonly BehaviourTree tree;

    public BehaviourTreeEntity(string name, Node rootNode) : base(name) {
        tree = new(rootNode);
        tree.Init(this);
    }

    public override void FixedUpdate() {
        tree.FixedUpdate();
    }

    public override void Update() {
        tree.Update();
    }

    public override void Start() {
        tree.Start();
    }
}