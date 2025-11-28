using System;
using UnityEngine;

namespace HoshinoLabs.Sardinject {
    public static class ComponentContainerBuilderExtensions {
        public static void UseEntryPoints(this ContainerBuilder self, Action<EntryPointsBuilder> configuration) {
            configuration(new EntryPointsBuilder(self));
        }

        public static void UseEntryPoints(this ContainerBuilder self, Transform transform, Action<EntryPointsBuilder> configuration) {
            configuration(new EntryPointsBuilder(self, transform));
        }

        public static void UseComponents(this ContainerBuilder self, Action<ComponentsBuilder> configuration) {
            configuration(new ComponentsBuilder(self));
        }

        public static void UseComponents(this ContainerBuilder self, Transform transform, Action<ComponentsBuilder> configuration) {
            configuration(new ComponentsBuilder(self, transform));
        }

        public static ComponentBindingBuilder RegisterEntryPoint(this ContainerBuilder self, Type type, Lifetime lifetime) {
            return self.RegisterComponent(type, lifetime)
                .EnsureBindingResolved(type, self);
        }

        public static ComponentBindingBuilder RegisterEntryPoint<T>(this ContainerBuilder self, Lifetime lifetime) where T : Component {
            return self.RegisterComponent<T>(lifetime)
                .EnsureBindingResolved<T>(self);
        }

        public static ComponentBindingBuilder RegisterComponent(this ContainerBuilder self, Type type, Lifetime lifetime) {
            var destination = new ComponentDestination();
            var resolverBuilder = new ComponentResolverBuilder(type, destination).OverrideScopeIfNeeded(self, lifetime);
            var builder = new ComponentBindingBuilder(type, resolverBuilder, destination);
            self.Register(builder);
            return builder;
        }

        public static ComponentBindingBuilder RegisterComponent<T>(this ContainerBuilder self, Lifetime lifetime) where T : Component {
            return self.RegisterComponent(typeof(T), lifetime);
        }

        public static ComponentBindingBuilder RegisterComponentInstance<T>(this ContainerBuilder self, T component) {
            var destination = new ComponentDestination();
            var resolverBuilder = new ExistenceComponentResolverBuilder(component.GetType(), component, destination).OverrideScopeIfNeeded(self, Lifetime.Cached);
            var builder = new ComponentBindingBuilder(component.GetType(), resolverBuilder, destination)
                .EnsureBindingResolved<T>(self);
            self.Register(builder);
            return builder;
        }

        public static ComponentBindingBuilder RegisterComponentInHierarchy(this ContainerBuilder self, Type type) {
            var destination = new ComponentDestination();
            var resolverBuilder = new FindComponentResolverBuilder(type, destination).OverrideScopeIfNeeded(self, Lifetime.Cached);
            var builder = new ComponentBindingBuilder(type, resolverBuilder, destination)
                .EnsureBindingResolved(type, self);
            self.Register(builder);
            return builder;
        }

        public static ComponentBindingBuilder RegisterComponentInHierarchy<T>(this ContainerBuilder self) where T : Component {
            return self.RegisterComponentInHierarchy(typeof(T));
        }

        public static ComponentBindingBuilder RegisterComponentInNewPrefab(this ContainerBuilder self, Type type, Lifetime lifetime, GameObject prefab) {
            var destination = new ComponentDestination();
            var resolverBuilder = new NewPrefabComponentResolverBuilder(type, prefab, destination).OverrideScopeIfNeeded(self, lifetime);
            var builder = new ComponentBindingBuilder(type, resolverBuilder, destination);
            self.Register(builder);
            return builder;
        }

        public static ComponentBindingBuilder RegisterComponentInNewPrefab<T>(this ContainerBuilder self, Lifetime lifetime, GameObject prefab) where T : Component {
            return self.RegisterComponentInNewPrefab(typeof(T), lifetime, prefab);
        }

        public static ComponentBindingBuilder RegisterComponentOnNewGameObject(this ContainerBuilder self, Type type, Lifetime lifetime, string gameObjectName = null) {
            var destination = new ComponentDestination();
            var resolverBuilder = new NewGameObjectComponentResolverBuilder(type, gameObjectName, destination).OverrideScopeIfNeeded(self, lifetime);
            var builder = new ComponentBindingBuilder(type, resolverBuilder, destination);
            self.Register(builder);
            return builder;
        }

        public static ComponentBindingBuilder RegisterComponentOnNewGameObject<T>(this ContainerBuilder self, Lifetime lifetime, string gameObjectName = null) where T : Component {
            return self.RegisterComponentOnNewGameObject(typeof(T), lifetime, gameObjectName);
        }
    }
}
