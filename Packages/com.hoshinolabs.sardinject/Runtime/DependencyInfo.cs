using System;
using System.Linq;

namespace HoshinoLabs.Sardinject {
    internal struct DependencyInfo {
        public readonly Binding Dest;
        public readonly Type DestType;
        public readonly Binding Src;
        public readonly Type SrcType;
        public readonly object Member;

        public DependencyInfo(Binding dest, Type destType) {
            Dest = dest;
            DestType = destType;
            Src = null;
            SrcType = null;
            Member = null;
        }

        public DependencyInfo(Binding dest, Type destType, Binding src, Type srcType, InjectConstructorInfo constructorInfo) {
            Dest = dest;
            DestType = destType;
            Src = src;
            SrcType = srcType;
            Member = constructorInfo;
        }

        public DependencyInfo(Binding dest, Type destType, Binding src, Type srcType, InjectFieldInfo fieldInfo) {
            Dest = dest;
            DestType = destType;
            Src = src;
            SrcType = srcType;
            Member = fieldInfo;
        }

        public DependencyInfo(Binding dest, Type destType, Binding src, Type srcType, InjectPropertyInfo propertyInfo) {
            Dest = dest;
            DestType = destType;
            Src = src;
            SrcType = srcType;
            Member = propertyInfo;
        }

        public DependencyInfo(Binding dest, Type destType, Binding src, Type srcType, InjectMethodInfo methodInfo) {
            Dest = dest;
            DestType = destType;
            Src = src;
            SrcType = srcType;
            Member = methodInfo;
        }

        public override string ToString() {
            switch (Member) {
                case InjectConstructorInfo constructorInfo: {
                        return $"{SrcType}..ctor({string.Join(", ", constructorInfo.Parameters.Select(x => x.Name))})";
                    }
                case InjectMethodInfo methodInfo: {
                        return $"{SrcType.FullName}.{methodInfo.Name}({string.Join(", ", methodInfo.Parameters.Select(x => x.Name))})";
                    }
                case InjectFieldInfo fieldInfo: {
                        return $"{SrcType.FullName}.{fieldInfo.Name}";
                    }
                case InjectPropertyInfo propertyInfo: {
                        return $"{SrcType.FullName}.{propertyInfo.Name}";
                    }
            }
            return string.Empty;
        }
    }
}
