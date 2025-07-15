using System;
using System.Linq;
using System.Reflection;

namespace HoshinoLabs.Sardinject.Udon {
    internal static class FieldInfoExtensions {
        public static InjectFieldInfo ToInjectField(this FieldInfo self) {
            var attribute = (IInject)self.GetCustomAttributes().Where(x => x.GetType().GetInterfaces().Contains(typeof(IInject))).FirstOrDefault();
            return new InjectFieldInfo(self, attribute?.Id);
        }
    }
}
