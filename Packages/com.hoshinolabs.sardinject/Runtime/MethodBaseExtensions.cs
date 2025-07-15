using System.Linq;
using System.Reflection;

namespace HoshinoLabs.Sardinject {
    internal static class MethodBaseExtensions {
        public static InjectParameterInfo[] GetInjectParameters(this MethodBase self) {
            return self.GetParameters()
                .Select(x => x.ToInjectParameter())
                .ToArray();
        }
    }
}
