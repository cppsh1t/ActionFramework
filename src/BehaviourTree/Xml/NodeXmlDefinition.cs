using System.Reflection;
using ActioinFramework.BehaviourTree.BehaviourNode;
using Newtonsoft.Json;
namespace ActioinFramework.BehaviourTree.Xml;

public class NodeXmlDefinition(Type nodeType, string nodeName) {

    public static readonly List<NodeXmlDefinition> allDefinitions = [];

    public static NodeXmlDefinition? GetDefinition(string name) {
        return allDefinitions.Find(item => item.nodeName == name);
    }

    public static void BuildJsonFile(string filePath) {
        string json = JsonConvert.SerializeObject(new { definitions = allDefinitions.Select(item => item.ToJson()) });
        File.WriteAllText(filePath, json);
    }

    static NodeXmlDefinition() {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] allTypes = assembly.GetTypes();
        IEnumerable<Type> availeTypes = allTypes.Where(item => typeof(Node).IsAssignableFrom(item) && item.GetCustomAttribute<NodeXmlNameAttribute>() != null);
        foreach (Type type in availeTypes) {
            string nodeName = type.GetCustomAttribute<NodeXmlNameAttribute>()!.name;
            NodeXmlDefinition definition = new(type, nodeName);
            var fieldAttrs = type.GetCustomAttributes<NodeXmlPropertyAttribute>();
            if (fieldAttrs == null) {
                allDefinitions.Add(definition);
                continue;
            }
            foreach (var fieldAttr in fieldAttrs) {
                string fieldName = fieldAttr.fieldName;
                string attrName = fieldAttr.attributeName;
                definition.AddFieldInfo(fieldAttr.attributeName, fieldAttr.fieldName);
            }
            allDefinitions.Add(definition);
        }
    }

    public readonly Type nodeType = nodeType;
    public readonly string nodeName = nodeName;
    public readonly Dictionary<string, FieldInfo> fieldMap = [];

    public void AddFieldInfo(string attributeName, string fieldName) {
        FieldInfo fieldInfo = nodeType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        ?? throw new MissingFieldException($"{nodeType.FullName}上不存在{fieldName}字段");
        fieldMap.Add(attributeName, fieldInfo);
    }

    private string GetBaseType() {
        if (typeof(ActionNode).IsAssignableFrom(nodeType)) {
            return "action";
        }
        if (typeof(CompositeNode).IsAssignableFrom(nodeType)) {
            return "composite";
        }
        if (typeof(DecoratorNode).IsAssignableFrom(nodeType)) {
            return "decorator";
        }
        throw new InvalidOperationException($"非法节点类型: {nodeType}");
    }

    public object ToJson() {
        return new {
            nodeName,
            fields = fieldMap.Keys.ToArray(),
            type = GetBaseType()
        };
    }
}