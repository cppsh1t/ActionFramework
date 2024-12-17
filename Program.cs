using ActioinFramework.BehaviourTree.BehaviourNode;
using ActioinFramework.BehaviourTree.Execution;
using ActioinFramework.BehaviourTree.Xml;
using ActioinFramework.GameEngine;

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

