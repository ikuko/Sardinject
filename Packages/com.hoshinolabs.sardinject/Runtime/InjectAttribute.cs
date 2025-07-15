using System;

namespace HoshinoLabs.Sardinject {
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class InjectAttribute : PreserveAttribute, IInject {
        readonly object id;

        public object Id => id;

        public InjectAttribute(object id = null) {
            this.id = id;
        }
    }
}
