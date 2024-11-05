namespace HoshinoLabs.Sardinject {
    public static class ResolverBuilderExtensions {
        public static IResolverBuilder OverrideScopeIfNeeded(this IResolverBuilder self, ContainerBuilder builder, Lifetime lifetime) {
            switch (lifetime) {
                case Lifetime.Cached: {
                        return new OverrideCachedScopeResolverBuilder(self, builder);
                    }
                case Lifetime.Scoped: {
                        return new OverrideSelfScopeResolverBuilder(self);
                    }
            }
            return self;
        }
    }
}