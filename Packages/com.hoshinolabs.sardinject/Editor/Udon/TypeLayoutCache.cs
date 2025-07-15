using System;
using System.Collections.Generic;

namespace HoshinoLabs.Sardinject.Udon {
    public static class TypeLayoutCache {
        static readonly Dictionary<Type, TypeLayout> cache = new();

        public static TypeLayout GetOrBuild(Type type) {
            if (!cache.TryGetValue(type, out var info)) {
                var lookupId = type.GetLookupId();
                var fieldInfos = type.GetInjectFields();
                var propertyInfos = type.GetInjectProperties(lookupId);
                var methodInfos = type.GetInjectMethods(lookupId);
                info = new TypeLayout(type, fieldInfos, propertyInfos, methodInfos);
                cache.Add(type, info);
            }
            return info;
        }
    }
}
