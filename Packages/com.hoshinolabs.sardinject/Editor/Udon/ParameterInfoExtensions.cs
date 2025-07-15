using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HoshinoLabs.Sardinject.Udon {
    public static class ParameterInfoExtensions {
        internal static InjectParameterInfo ToInjectParameter(this ParameterInfo self, Dictionary<object, int> lookupId) {
            var attribute = (IInject)self.GetCustomAttributes().Where(x => x.GetType().GetInterfaces().Contains(typeof(IInject))).FirstOrDefault();
            return new InjectParameterInfo(self, self.GetSymbol(lookupId), attribute?.Id);
        }

        internal static string GetSymbol(this ParameterInfo self, Dictionary<object, int> lookupId) {
            return $"__{lookupId[self]}_{self.Name}";
        }

        public static string GetUdonExportSymbol(this ParameterInfo self, Type declaringType) {
            var lookupId = declaringType.GetLookupId();
            return self.GetSymbol(lookupId);
        }
    }
}
