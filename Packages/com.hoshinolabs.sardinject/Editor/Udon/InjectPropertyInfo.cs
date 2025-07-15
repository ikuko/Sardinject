using System;
using System.Reflection;

namespace HoshinoLabs.Sardinject.Udon {
    public sealed class InjectPropertyInfo {
        readonly PropertyInfo propertyInfo;
        readonly InjectMethodInfo getter;
        readonly InjectMethodInfo setter;
        readonly object id;

        public string Name => propertyInfo.Name;
        public Type PropertyType => propertyInfo.PropertyType;
        public bool CanRead => getter != null;
        public bool CanWrite => setter != null;
        public InjectMethodInfo Getter => getter;
        public InjectMethodInfo Setter => setter;
        public object Id => id;

        public InjectPropertyInfo(PropertyInfo propertyInfo, InjectMethodInfo getter, InjectMethodInfo setter, object id) {
            this.propertyInfo = propertyInfo;
            this.getter = getter;
            this.setter = setter;
            this.id = id;
        }
    }
}
