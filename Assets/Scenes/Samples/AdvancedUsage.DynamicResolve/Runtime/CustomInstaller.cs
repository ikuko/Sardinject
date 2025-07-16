using HoshinoLabs.Sardinject;
using UnityEngine;

namespace Samples.AdvancedUsage.DynamicResolve {
    public class CustomInstaller : MonoBehaviour, IInstaller {
        public void Install(ContainerBuilder builder) {
            builder.RegisterComponentOnNewGameObject<Sardine>(Lifetime.Cached);
            builder.RegisterEntryPoint<StartupGreeting>(Lifetime.Cached);
        }
    }
}
