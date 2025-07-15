using System.Reflection;

namespace HoshinoLabs.Sardinject {
    public sealed class InjectMethodInfo {
        readonly MethodInfo methodInfo;
        readonly InjectParameterInfo[] parameters;

        public string Name => methodInfo.Name;
        public InjectParameterInfo[] Parameters => parameters;

        public InjectMethodInfo(MethodInfo methodInfo, InjectParameterInfo[] parameters) {
            this.methodInfo = methodInfo;
            this.parameters = parameters;
        }

        public object Invoke(object obj, object[] parameters) {
            return methodInfo.Invoke(obj, parameters);
        }
    }
}
