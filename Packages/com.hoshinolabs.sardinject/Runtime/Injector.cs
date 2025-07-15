using System.Collections.Generic;
using System.Linq;

namespace HoshinoLabs.Sardinject {
    public sealed class Injector {
        public readonly TypeLayout TypeInfo;

        public Injector(TypeLayout typeInfo) {
            TypeInfo = typeInfo;
        }

        public object Construct(Container container, IReadOnlyDictionary<object, IResolver> parameters) {
            var values = TypeInfo.Constructor.Parameters
                .Select(x => container.ResolveOrParameterOrId(x.Name, x.ParameterType, x.Id, parameters))
                .ToArray();
            var instance = TypeInfo.Constructor.Invoke(values);
            Inject(instance, container, parameters);
            return instance;
        }

        public void Inject(object instance, Container container, IReadOnlyDictionary<object, IResolver> parameters) {
            InjectFields(instance, container, parameters);
            InjectProperties(instance, container, parameters);
            InjectMethods(instance, container, parameters);
#if UDONSHARP
            if (typeof(UdonSharp.UdonSharpBehaviour).IsAssignableFrom(instance.GetType())) {
                UdonSharpBehaviourExtensions.ApplyProxyModifications((UdonSharp.UdonSharpBehaviour)instance);
            }
#endif
        }

        void InjectFields(object instance, Container container, IReadOnlyDictionary<object, IResolver> parameters) {
            foreach (var field in TypeInfo.Fields) {
                var value = container.ResolveOrParameterOrId(field.Name, field.FieldType, field.Id, parameters);
                field.SetValue(instance, value);
            }
        }

        void InjectProperties(object instance, Container container, IReadOnlyDictionary<object, IResolver> parameters) {
            foreach (var property in TypeInfo.Properties) {
                var value = container.ResolveOrParameterOrId(property.Name, property.PropertyType, property.Id, parameters);
                property.SetValue(instance, value);
            }
        }

        void InjectMethods(object instance, Container container, IReadOnlyDictionary<object, IResolver> parameters) {
            foreach (var method in TypeInfo.Methods) {
                var values = method.Parameters
                    .Select(x => container.ResolveOrParameterOrId(x.Name, x.ParameterType, x.Id, parameters))
                    .ToArray();
                method.Invoke(instance, values);
            }
        }
    }
}
