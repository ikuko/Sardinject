using HoshinoLabs.Sardinject;
using Samples.BasicUsage.Udon;
using UnityEngine;

namespace Samples.BasicUsage {
    [AddComponentMenu("")]
    public class CustomInstaller : MonoBehaviour, IInstaller {
        public void Install(ContainerBuilder builder) {
            builder.RegisterComponentOnNewGameObject<Sardine>(Lifetime.Cached);
            builder.RegisterEntryPoint<StartupGreeting>(Lifetime.Cached);
        }
    }
}
