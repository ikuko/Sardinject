using System;
using System.Reflection;

namespace HoshinoLabs.Sardinject.Udon {
    public sealed class InjectParameterInfo {
        readonly ParameterInfo parameterInfo;
        readonly string symbol;
        readonly object id;

        public string Name => parameterInfo.Name;
        public Type ParameterType => parameterInfo.ParameterType;
        public string Symbol => symbol;
        public object Id => id;

        public InjectParameterInfo(ParameterInfo parameterInfo, string symbol, object id) {
            this.parameterInfo = parameterInfo;
            this.symbol = symbol;
            this.id = id;
        }
    }
}
