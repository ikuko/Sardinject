using System;
using System.Collections.Generic;
using System.Linq;

namespace HoshinoLabs.Sardinject {
    internal class RegistryBuilder {
        readonly Registry registry = new();
        readonly List<BindingBuilder> builders = new();

        public RegistryBuilder() {

        }

        public RegistryBuilder(Registry registry) {
            this.registry = registry;
        }

        public void Register<T>(T builder) where T : BindingBuilder {
            builders.Add(builder);
        }

        public Registry Build() {
            var data = builders
                .ToDictionary(x => x.Build(), x => x.InterfaceTypes.Concat(new[] { x.ImplementationType }).ToList());
            var registry = BuildRegistry(data);
            ValidateCircularDependencies(data, registry);
            return registry;
        }

        Registry BuildRegistry(Dictionary<Binding, List<Type>> data) {
            var bindings = data
                .SelectMany(x => x.Value.Select(t => (t, x: x.Key)))
                .GroupBy(x => x.t, x => x.x)
                .ToDictionary(x => x.Key, x => x.ToList());
            bindings = registry.Bindings
                .Concat(bindings)
                .GroupBy(x => x.Key, x => x.Value)
                .ToDictionary(x => x.Key, x => x.SelectMany(x => x).ToList());
            return new Registry(bindings);
        }

        void ValidateCircularDependencies(Dictionary<Binding, List<Type>> data, Registry registry) {
            foreach (var (binding, types) in data) {
                var dependency = new DependencyInfo(binding, types.Last());
                var dependencies = new Stack<DependencyInfo>();
                ValidateCircularDependencies(dependency, registry, dependencies);
            }
        }

        void ValidateCircularDependencies(DependencyInfo current, Registry registry, Stack<DependencyInfo> dependencies) {
            ValidateCircularDependencies(current, dependencies);
            dependencies.Push(current);
            var info = TypeLayoutCache.GetOrBuild(current.DestType);
            ValidateCircularDependencies(current, registry, dependencies, info);
            dependencies.Pop();
        }

        void ValidateCircularDependencies(DependencyInfo current, Stack<DependencyInfo> dependencies) {
            foreach (var dependency in dependencies.Select((v, i) => (v, i))) {
                if (current.DestType != dependency.v.DestType) {
                    continue;
                }
                dependencies.Push(current);
                var messages = dependencies
                    .Take(dependency.i + 1)
                    .Select((v, i) => $"[{i}] `{v.DestType.FullName}` (at `{v}`)")
                    .ToList();
                messages.Insert(0, "Circular dependency was detected.");
                throw new SardinjectException(string.Join(Environment.NewLine, messages));
            }
        }

        void ValidateCircularDependencies(DependencyInfo current, Registry registry, Stack<DependencyInfo> dependencies, TypeLayout info) {
            ValidateCircularDependencies(current, registry, dependencies, info.Constructor);
            foreach (var fieldInfo in info.Fields) {
                ValidateCircularDependencies(current, registry, dependencies, fieldInfo);
            }
            foreach (var propertyInfo in info.Properties) {
                ValidateCircularDependencies(current, registry, dependencies, propertyInfo);
            }
            foreach (var methodInfo in info.Methods) {
                ValidateCircularDependencies(current, registry, dependencies, methodInfo);
            }
        }

        void ValidateCircularDependencies(DependencyInfo current, Registry registry, Stack<DependencyInfo> dependencies, InjectConstructorInfo constructorInfo) {
            foreach (var parameterInfo in constructorInfo.Parameters) {
                foreach (var binding in registry.GetBindings(parameterInfo.ParameterType)) {
                    var dependency = new DependencyInfo(binding, parameterInfo.ParameterType, current.Dest, current.DestType, constructorInfo);
                    ValidateCircularDependencies(dependency, registry, dependencies);
                }
            }
        }

        void ValidateCircularDependencies(DependencyInfo current, Registry registry, Stack<DependencyInfo> dependencies, InjectFieldInfo fieldInfo) {
            foreach (var binding in registry.GetBindings(fieldInfo.FieldType)) {
                var dependency = new DependencyInfo(binding, fieldInfo.FieldType, current.Dest, current.DestType, fieldInfo);
                ValidateCircularDependencies(dependency, registry, dependencies);
            }
        }

        void ValidateCircularDependencies(DependencyInfo current, Registry registry, Stack<DependencyInfo> dependencies, InjectPropertyInfo propertyInfo) {
            foreach (var binding in registry.GetBindings(propertyInfo.PropertyType)) {
                var dependency = new DependencyInfo(binding, propertyInfo.PropertyType, current.Dest, current.DestType, propertyInfo);
                ValidateCircularDependencies(dependency, registry, dependencies);
            }
        }

        void ValidateCircularDependencies(DependencyInfo current, Registry registry, Stack<DependencyInfo> dependencies, InjectMethodInfo methodInfo) {
            foreach (var parameterInfo in methodInfo.Parameters) {
                foreach (var binding in registry.GetBindings(parameterInfo.ParameterType)) {
                    var dependency = new DependencyInfo(binding, parameterInfo.ParameterType, current.Dest, current.DestType, methodInfo);
                    ValidateCircularDependencies(dependency, registry, dependencies);
                }
            }
        }
    }
}
