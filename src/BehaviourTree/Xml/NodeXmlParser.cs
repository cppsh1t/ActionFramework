using System.Reflection;
using System.Xml;
using ActioinFramework.BehaviourTree.BehaviourNode;

namespace ActioinFramework.BehaviourTree.Xml;

public class NodeXmlParser {

    private Node MakeNode(XmlNode xmlNode) {
        NodeXmlDefinition definition = NodeXmlDefinition.GetDefinition(xmlNode.Name)
        ?? throw new InvalidOperationException($"不存在{xmlNode.Name}节点类型");

        ConstructorInfo constructorInfo = definition.nodeType.GetConstructor(Type.EmptyTypes)
        ?? throw new MissingMethodException($"{definition.nodeType.FullName}上不存在无参构造函数");

        Node node = (Node)constructorInfo.Invoke(null);

        if (xmlNode.Attributes == null) return node;

        foreach (XmlAttribute xmlAttribute in xmlNode.Attributes) {
            string attrName = xmlAttribute.Name;
            if (attrName == "x" || attrName == "y") continue;
            string attrValue = xmlAttribute.Value;
            FieldInfo fieldInfo = definition.fieldMap[attrName];
            Type fieldType = fieldInfo.FieldType;
            object fieldValue = Convert.ChangeType(attrValue, fieldType);
            fieldInfo.SetValue(node, fieldValue);
        }

        MakeTree(node, xmlNode);
        return node;
    }

    private void MakeTree(Node node, XmlNode xmlNode) {
        var xmlChildNodes = xmlNode.ChildNodes;
        Type nodeType = node.GetType();
        if (xmlChildNodes.Count == 0) return;
        if (typeof(CompositeNode).IsAssignableFrom(nodeType)) {
            CompositeNode castedNode = (CompositeNode)node;
            foreach (XmlNode childXmlNode in xmlNode.ChildNodes) {
                Node childNode = MakeNode(childXmlNode);
                castedNode.AddNode(childNode);
            }
        }
        else if (typeof(DecoratorNode).IsAssignableFrom(nodeType)) {
            if (xmlChildNodes.Count != 1) throw new InvalidOperationException($"该节点类型无法拥有1个以上子节点: {nodeType.FullName}");
            DecoratorNode castedNode = (DecoratorNode)node;
            foreach (XmlNode childXmlNode in xmlNode.ChildNodes) {
                Node childNode = MakeNode(childXmlNode);
                castedNode.SetChild(childNode);
            }
        }
        else {
            throw new InvalidOperationException($"该节点类型无法拥有子节点: {nodeType.FullName}");
        }
    }

    public Node Parse(string path) {
        XmlDocument xmlDoc = new();
        xmlDoc.Load(path);
        XmlNode rootNode = xmlDoc.DocumentElement ?? throw new XmlException("Xml 结构错误");
        if (rootNode.Name != "root") throw new XmlException("Xml 结构错误");
        if (rootNode.ChildNodes.Count != 1) throw new XmlException("Xml 结构错误");
        XmlNode xmlFirstNode = rootNode.FirstChild ?? throw new XmlException("Xml 结构错误"); ;
        Node firstNode = MakeNode(xmlFirstNode);
        return firstNode;
    }

}