using System;
using System.Reflection;

namespace HoshinoLabs.Sardinject.Udon {
    public sealed class InjectFieldInfo {
        readonly FieldInfo fieldInfo;
        readonly object id;

        public string Name => fieldInfo.Name;
        public Type FieldType => fieldInfo.FieldType;
        public object Id => id;

        public InjectFieldInfo(FieldInfo fieldInfo, object id) {
            this.fieldInfo = fieldInfo;
            this.id = id;
        }
    }
}
