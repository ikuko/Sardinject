using HoshinoLabs.Sardinject;
using Samples.AdvancedUsage.DynamicResolve.Udon;
using UnityEngine;

namespace Samples.AdvancedUsage.DynamicResolve {
    [AddComponentMenu("")]
    public class CustomInstaller : MonoBehaviour, IInstaller {
        public void Install(ContainerBuilder builder) {
            builder.RegisterComponentOnNewGameObject<Sardine>(Lifetime.Cached);
            builder.RegisterEntryPoint<StartupGreeting>(Lifetime.Cached);
        }
    }
}
