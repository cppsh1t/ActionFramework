using ActioinFramework.BehaviourTree.BehaviourNode;
using ActioinFramework.BehaviourTree.Execution;
using ActioinFramework.GameEngine;

World world = new(500, 500, TimeSpan.FromSeconds(20));
SequenceNode sequenceNode = new();
sequenceNode.AddNode(new WaitNode(2));
sequenceNode.AddNode(new HealNode());
BehaviourTreeEntity entity = new("heal-man", sequenceNode);
entity.Health = 50;
world.AddEntity(entity);
await world.Log();

