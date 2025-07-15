using System.Linq;
using System.Reflection;

namespace HoshinoLabs.Sardinject {
    internal static class MethodBaseExtensions {
        public static InjectParameterInfo[] GetInjectParameters(this MethodBase self) {
            return self.GetParameters()
                .Select(x => new InjectParameterInfo(x, ((IInject)x.GetCustomAttributes().Where(x => x.GetType().GetInterfaces().Contains(typeof(IInject))).FirstOrDefault())?.Id))
                .ToArray();
        }
    }
}
