using System;
using System.Collections.Generic;

namespace HoshinoLabs.Sardinject {
    public static class TypeLayoutCache {
        static readonly Dictionary<Type, TypeLayout> cache = new();

        public static TypeLayout GetOrBuild(Type type) {
            if (!cache.TryGetValue(type, out var info)) {
                var constructorInfo = type.GetInjectConstructor();
                var fieldInfos = type.GetInjectFields();
                var propertyInfos = type.GetInjectProperties();
                var methodInfos = type.GetInjectMethods();
                info = new TypeLayout(type, constructorInfo, fieldInfos, propertyInfos, methodInfos);
                cache.Add(type, info);
            }
            return info;
        }
    }
}
