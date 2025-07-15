using System;

namespace HoshinoLabs.Sardinject.Udon {
    public sealed class TypeLayout {
        public readonly Type Type;
        public readonly InjectFieldInfo[] Fields;
        public readonly InjectPropertyInfo[] Properties;
        public readonly InjectMethodInfo[] Methods;

        public TypeLayout(Type type, InjectFieldInfo[] fieldInfos, InjectPropertyInfo[] propertyInfos, InjectMethodInfo[] methodInfos) {
            Type = type;
            Fields = fieldInfos;
            Properties = propertyInfos;
            Methods = methodInfos;
        }
    }
}
