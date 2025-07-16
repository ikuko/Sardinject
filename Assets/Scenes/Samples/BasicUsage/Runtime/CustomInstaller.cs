using HoshinoLabs.Sardinject;
using UnityEngine;

namespace Samples.BasicUsage {
    public class CustomInstaller : MonoBehaviour, IInstaller {
        public void Install(ContainerBuilder builder) {
            builder.RegisterComponentOnNewGameObject<Sardine>(Lifetime.Cached);
            builder.RegisterEntryPoint<StartupGreeting>(Lifetime.Cached);
        }
    }
}
