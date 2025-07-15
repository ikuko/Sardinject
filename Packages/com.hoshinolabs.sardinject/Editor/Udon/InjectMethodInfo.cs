using System.Reflection;

namespace HoshinoLabs.Sardinject.Udon {
    public sealed class InjectMethodInfo {
        readonly MethodInfo methodInfo;
        readonly string symbol;
        readonly InjectParameterInfo[] parameters;

        public string Name => methodInfo.Name;
        public string Symbol => symbol;
        public InjectParameterInfo[] Parameters => parameters;


        public InjectMethodInfo(MethodInfo methodInfo, string symbol, InjectParameterInfo[] parameters) {
            this.methodInfo = methodInfo;
            this.symbol = symbol;
            this.parameters = parameters;
        }
    }
}
