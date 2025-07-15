using System.Reflection;

namespace HoshinoLabs.Sardinject {
    internal static class ConstructorInfoExtensions {
        public static InjectConstructorInfo ToInjectConstructor(this ConstructorInfo self) {
            return new InjectConstructorInfo(self, self.GetInjectParameters());
        }
    }
}
