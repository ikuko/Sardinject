using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HoshinoLabs.Sardinject.Udon {
    internal static class PropertyInfoExtensions {
        public static InjectPropertyInfo ToInjectProperty(this PropertyInfo self, Dictionary<object, int> lookupId) {
            var attribute = (IInject)self.GetCustomAttributes().Where(x => x.GetType().GetInterfaces().Contains(typeof(IInject))).FirstOrDefault();
            var getter = self.CanRead ? new InjectMethodInfo(self.GetMethod, self.GetMethod.GetSymbol(lookupId), self.GetMethod.GetInjectParameters(lookupId)) : null;
            var setter = self.CanWrite ? new InjectMethodInfo(self.SetMethod, self.SetMethod.GetSymbol(lookupId), self.SetMethod.GetInjectParameters(lookupId)) : null;
            return new InjectPropertyInfo(self, getter, setter, attribute?.Id);
        }
    }
}
