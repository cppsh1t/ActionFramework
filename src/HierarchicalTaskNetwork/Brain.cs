namespace ActioinFramework.HierarchicalTaskNetwork;

public class Brain {
    private Dictionary<string, object> strMap = [];
    private Dictionary<Type, object> typeMap = [];

    public Brain() {

    }

    public Brain Clone() {
        Brain brain = new() {
            strMap = new(strMap),
            typeMap = new(typeMap)
        };
        return brain;
    }

    public object? Reslove(string name) {
        return strMap.GetValueOrDefault(name);
    }

    public T? Reslove<T>() {
        return (T?)typeMap.GetValueOrDefault(typeof(T));
    }

    public void Put(string name, object target) {
        strMap[name] = target;
    }

    public void Put<T>(object target) {
        typeMap[typeof(T)] = target;
    }
}