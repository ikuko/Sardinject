using UdonSharp;
using UnityEngine;

namespace Samples.AdvancedUsage.DynamicResolve.Udon {
    [AddComponentMenu("")]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Sardine : UdonSharpBehaviour {
        public void Hello() {
            Debug.Log($"Hello. Do you like sardines?");
        }
    }
}
