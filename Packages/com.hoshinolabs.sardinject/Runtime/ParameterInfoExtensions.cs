using System;
using System.Linq;
using System.Reflection;

namespace HoshinoLabs.Sardinject {
    internal static class ParameterInfoExtensions {
        public static InjectParameterInfo ToInjectParameter(this ParameterInfo self) {
            var attribute = (IInject)self.GetCustomAttributes().Where(x => x.GetType().GetInterfaces().Contains(typeof(IInject))).FirstOrDefault();
            return new InjectParameterInfo(self, attribute?.Id);
        }
    }
}
