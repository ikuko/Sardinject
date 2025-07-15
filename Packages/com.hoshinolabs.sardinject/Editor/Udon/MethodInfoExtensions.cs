using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HoshinoLabs.Sardinject.Udon {
    public static class MethodInfoExtensions {
        internal static InjectMethodInfo ToInjectMethod(this MethodInfo self, Dictionary<object, int> lookupId) {
            return new InjectMethodInfo(self, self.GetSymbol(lookupId), self.GetInjectParameters(lookupId));
        }

        internal static InjectParameterInfo[] GetInjectParameters(this MethodInfo self, Dictionary<object, int> lookupId) {
            return self.GetParameters()
                .Select(x => x.ToInjectParameter(lookupId))
                .ToArray();
        }

        internal static string GetSymbol(this MethodInfo self, Dictionary<object, int> lookupId) {
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

        public static string GetUdonExportSymbol(this MethodInfo self, Type declaringType) {
            var lookupId = declaringType.GetLookupId();
            return self.GetSymbol(lookupId);
        }
    }
}
