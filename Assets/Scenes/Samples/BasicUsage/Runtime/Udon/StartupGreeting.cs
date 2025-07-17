using HoshinoLabs.Sardinject.Udon;
using UdonSharp;
using UnityEngine;

namespace Samples.BasicUsage.Udon {
    [AddComponentMenu("")]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class StartupGreeting : UdonSharpBehaviour {
        [Inject, SerializeField, HideInInspector]
        Sardine sardine;

        private void Start() {
            sardine.Hello();
        }
    }
}
