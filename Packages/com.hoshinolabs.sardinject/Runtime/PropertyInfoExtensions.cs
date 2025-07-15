using System;
using System.Linq;
using System.Reflection;

namespace HoshinoLabs.Sardinject {
    internal static class PropertyInfoExtensions {
        public static InjectPropertyInfo ToInjectProperty(this PropertyInfo self) {
            var attribute = (IInject)self.GetCustomAttributes().Where(x => x.GetType().GetInterfaces().Contains(typeof(IInject))).FirstOrDefault();
            return new InjectPropertyInfo(self, attribute?.Id);
        }
    }
}
