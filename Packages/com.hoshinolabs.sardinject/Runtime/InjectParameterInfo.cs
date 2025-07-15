using System;
using System.Reflection;

namespace HoshinoLabs.Sardinject {
    public sealed class InjectParameterInfo {
        readonly ParameterInfo parameterInfo;
        readonly object id;

        public string Name => parameterInfo.Name;
        public Type ParameterType => parameterInfo.ParameterType;
        public object Id => id;

        public InjectParameterInfo(ParameterInfo parameterInfo, object id) {
            this.parameterInfo = parameterInfo;
            this.id = id;
        }
    }
}
