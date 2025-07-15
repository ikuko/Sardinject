using System.Collections.Generic;
using System.Reflection;

namespace HoshinoLabs.Sardinject.Udon {
    internal static class ParameterInfoExtensions {
        public static string GetSymbol(this ParameterInfo self, Dictionary<object, int> lookupId) {
            return $"__{lookupId[self]}_{self.Name}";
        }
    }
}
