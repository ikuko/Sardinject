using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HoshinoLabs.Sardinject {
    internal static class TypeExtensions {
        public static InjectConstructorInfo GetInjectConstructor(this Type self) {
            var constructorInfos = self.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .OrderBy(x => x.IsDefined(typeof(InjectAttribute), false))
                .ThenByDescending(x => x.GetParameters().Length);
            if (1 < constructorInfos.Count(x => x.IsDefined(typeof(InjectAttribute), false))) {
                throw new SardinjectException("Multiple constructors with the Inject attribute were found.");
            }
            var constructorInfo = constructorInfos.First();
            return new InjectConstructorInfo(constructorInfo, constructorInfo.GetInjectParameters());
        }

        public static InjectFieldInfo[] GetInjectFields(this Type self) {
            return self.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.IsDefined(typeof(InjectAttribute), false))
                .Select(x => new InjectFieldInfo(x, x.GetCustomAttribute<InjectAttribute>().Id))
                .Concat(self.BaseType?.GetInjectFields() ?? Array.Empty<InjectFieldInfo>())
                .ToArray();
        }

        public static InjectPropertyInfo[] GetInjectProperties(this Type self) {
            return self.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.IsDefined(typeof(InjectAttribute), false) && x.CanWrite)
                .Select(x => new InjectPropertyInfo(x, x.GetCustomAttribute<InjectAttribute>().Id))
                .Concat(self.BaseType?.GetInjectProperties() ?? Array.Empty<InjectPropertyInfo>())
                .ToArray();
        }

        public static InjectMethodInfo[] GetInjectMethods(this Type self) {
            return self.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.IsDefined(typeof(InjectAttribute), false))
                .Select(x => new InjectMethodInfo(x, x.GetInjectParameters()))
                .Concat(self.BaseType?.GetInjectMethods() ?? Array.Empty<InjectMethodInfo>())
                .ToArray();
        }

        public static bool IsEnumerable(this Type self) {
            return self.IsGenericType
                && self.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }
    }
}
