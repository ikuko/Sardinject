using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HoshinoLabs.Sardinject.Udon {
    internal static class TypeExtensions {
        public static InjectFieldInfo[] GetInjectFields(this Type self) {
            return self.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.IsDefined(typeof(InjectAttribute), false))
                .Select(x => new InjectFieldInfo(x, x.GetCustomAttribute<InjectAttribute>().Id))
                .Concat(self.BaseType?.GetInjectFields() ?? Array.Empty<InjectFieldInfo>())
                .ToArray();
        }

        public static InjectPropertyInfo[] GetInjectProperties(this Type self, Dictionary<object, int> lookupId) {
            return self.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.IsDefined(typeof(InjectAttribute), false) && x.CanWrite)
                .Select(x => {
                    var getter = x.CanRead ? new InjectMethodInfo(x.GetMethod, x.GetMethod.GetSymbol(lookupId), x.GetMethod.GetInjectParameters(lookupId)) : null;
                    var setter = x.CanWrite ? new InjectMethodInfo(x.SetMethod, x.SetMethod.GetSymbol(lookupId), x.SetMethod.GetInjectParameters(lookupId)) : null;
                    return new InjectPropertyInfo(x, getter, setter, x.GetCustomAttribute<InjectAttribute>().Id);
                })
                .Concat(self.BaseType?.GetInjectProperties(lookupId) ?? Array.Empty<InjectPropertyInfo>())
                .ToArray();
        }

        public static Dictionary<object, int> GetLookupId(this Type self) {
            var methods = self.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .ToArray();
            var methodIds = methods
                .Where(x => 0 < x.GetParameters().Length)
                .GroupBy(x => x.Name)
                .SelectMany(x => x.Select((x, i) => (x, i)))
                .Select(x => ((object, int))x);
            var parameterIds = methods
                .SelectMany(x => x.GetParameters())
                .GroupBy(x => x.Name)
                .SelectMany(x => x.Select((x, i) => (x, i)))
                .Select(x => ((object, int))x);
            return methodIds.Concat(parameterIds)
                .ToDictionary(x => x.Item1, x => x.Item2);
        }

        public static InjectMethodInfo[] GetInjectMethods(this Type self, Dictionary<object, int> lookupId) {
            return self.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.IsDefined(typeof(InjectAttribute), false))
                .Select(x => new InjectMethodInfo(x, x.GetSymbol(lookupId), x.GetInjectParameters(lookupId)))
                .Concat(self.BaseType?.GetInjectMethods(lookupId) ?? Array.Empty<InjectMethodInfo>())
                .ToArray();
        }
    }
}
