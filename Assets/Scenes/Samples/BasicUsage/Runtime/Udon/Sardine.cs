using UdonSharp;
using UnityEngine;

namespace Samples.BasicUsage.Udon {
    [AddComponentMenu("")]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Sardine : UdonSharpBehaviour {
        public void Hello() {
            Debug.Log($"Hello. Do you like sardines?");
        }
    }
}
