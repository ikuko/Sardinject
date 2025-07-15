using System.Reflection;

namespace HoshinoLabs.Sardinject {
    internal static class MethodInfoExtensions {
        public static InjectMethodInfo ToInjectMethod(this MethodInfo self) {
            return new InjectMethodInfo(self, self.GetInjectParameters());
        }
    }
}
