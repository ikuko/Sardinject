using HoshinoLabs.Sardinject.Udon;
using UdonSharp;
using UnityEngine;

namespace Samples.AdvancedUsage.DynamicResolve {
    public class StartupGreeting : UdonSharpBehaviour {
        [Inject, SerializeField, HideInInspector]
        Container container;

        private void Start() {
            var sardine = container.Resolve<Sardine>();
            sardine.Hello();
        }
    }
}
