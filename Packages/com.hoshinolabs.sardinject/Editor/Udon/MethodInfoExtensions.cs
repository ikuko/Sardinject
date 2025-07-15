using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HoshinoLabs.Sardinject.Udon {
    internal static class MethodInfoExtensions {
        public static InjectParameterInfo[] GetInjectParameters(this MethodInfo self, Dictionary<object, int> lookupId) {
            return self.GetParameters()
                .Select(x => new InjectParameterInfo(x, x.GetSymbol(lookupId), ((IInject)x.GetCustomAttributes().Where(x => x.GetType().GetInterfaces().Contains(typeof(IInject))).FirstOrDefault())?.Id))
                .ToArray();
        }

        public static string GetSymbol(this MethodInfo self, Dictionary<object, int> lookupId) {
#if VRC_SDK_WORLDS3_8_1_OR_NEWER
            if (self.IsDefined(typeof(VRC.SDK3.UdonNetworkCalling.NetworkCallableAttribute), false)) {
                return self.Name;
            }
#endif
            if (lookupId.TryGetValue(self, out var id)) {
                return $"__{id}_{self.Name}";
            }
            return self.Name;
        }
    }
}
