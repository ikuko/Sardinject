using System;
using System.Reflection;

namespace HoshinoLabs.Sardinject {
    public sealed class InjectPropertyInfo {
        readonly PropertyInfo propertyInfo;
        readonly object id;

        public string Name => propertyInfo.Name;
        public Type PropertyType => propertyInfo.PropertyType;
        public object Id => id;

        public InjectPropertyInfo(PropertyInfo propertyInfo, object id) {
            this.propertyInfo = propertyInfo;
            this.id = id;
        }

        public void SetValue(object obj, object value) {
            propertyInfo.SetValue(obj, value);
        }
    }
}
