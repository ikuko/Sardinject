using System;

namespace HoshinoLabs.Sardinject.Udon {
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class InjectAttribute : Sardinject.InjectAttribute {
        public InjectAttribute(object id = null)
            : base(id) {

        }
    }
}
