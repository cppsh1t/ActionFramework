using System.Reflection;

namespace ActioinFramework.BehaviourTree.Xml;

public class NodeXmlDefinition(Type nodeType, string nodeName) {
    public readonly Type nodeType = nodeType;
    public readonly string nodeName = nodeName;
    public readonly Dictionary<string, FieldInfo> fieldMap = [];

    public void AddFieldInfo(string attributeName, string fieldName) {
        FieldInfo fieldInfo = nodeType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException($"{nodeType.FullName}上不存在{fieldName}字段");
        fieldMap.Add(attributeName, fieldInfo);
    }
}