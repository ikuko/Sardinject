using System;

namespace HoshinoLabs.Sardinject {
    public sealed class TypeLayout {
        public readonly Type Type;
        public readonly InjectConstructorInfo Constructor;
        public readonly InjectFieldInfo[] Fields;
        public readonly InjectPropertyInfo[] Properties;
        public readonly InjectMethodInfo[] Methods;

        public TypeLayout(Type type, InjectConstructorInfo constructorInfo, InjectFieldInfo[] fieldInfos, InjectPropertyInfo[] propertyInfos, InjectMethodInfo[] methodInfos) {
            Type = type;
            Constructor = constructorInfo;
            Fields = fieldInfos;
            Properties = propertyInfos;
            Methods = methodInfos;
        }
    }
}
