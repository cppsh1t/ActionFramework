using ActioinFramework.BehaviourTree.BehaviourNode;
using ActioinFramework.BehaviourTree.Execution;
using ActioinFramework.GameEngine;

World world = new(500, 500, TimeSpan.FromSeconds(60));
SequenceNode sequenceNode = new();
sequenceNode.AddNode(new WaitNode(2));
sequenceNode.AddNode(new HealNode());
RepeatNode repeatNode = new();
repeatNode.SetChild(sequenceNode);
BehaviourTreeEntity entity = new("heal-man", repeatNode) {
    Health = 50
};
world.AddEntity(entity);
await world.Log();

