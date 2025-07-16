using UdonSharp;
using UnityEngine;

namespace Samples.BasicUsage {
    public class Sardine : UdonSharpBehaviour {
        public void Hello() {
            Debug.Log($"Hello. Do you like sardines?");
        }
    }
}
