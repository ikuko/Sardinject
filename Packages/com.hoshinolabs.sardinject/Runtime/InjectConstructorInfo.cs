using System.Reflection;

namespace HoshinoLabs.Sardinject {
    public sealed class InjectConstructorInfo {
        readonly ConstructorInfo constructorInfo;
        readonly InjectParameterInfo[] parameters;

        public ConstructorInfo ConstructorInfo => constructorInfo;
        public InjectParameterInfo[] Parameters => parameters;

        public InjectConstructorInfo(ConstructorInfo constructorInfo, InjectParameterInfo[] parameters) {
            this.constructorInfo = constructorInfo;
            this.parameters = parameters;
        }

        public object Invoke(object[] parameters) {
            return constructorInfo.Invoke(parameters);
        }
    }
}
