using UdonSharp;
using UnityEngine;

namespace Samples.AdvancedUsage.DynamicResolve {
    public class Sardine : UdonSharpBehaviour {
        public void Hello() {
            Debug.Log($"Hello. Do you like sardines?");
        }
    }
}
