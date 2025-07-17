using HoshinoLabs.Sardinject.Udon;
using UdonSharp;
using UnityEngine;

namespace Samples.MoneyManage.Udon {
    /*
     * TheGameはゲームのエントリポイントであり、ゲームのロジックを管理します。
     * ユーザーがインタラクトすると、MoneyServiceから100を消費します。
     */
    [AddComponentMenu("")]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TheGame : UdonSharpBehaviour {
        [Inject, SerializeField, HideInInspector]
        MoneyService moneyService;
        [Inject, SerializeField, HideInInspector]
        MeshRenderer meshRenderer;
        [Inject(id: "gray"), SerializeField, HideInInspector]
        Material grayMaterial;
        [Inject(id: "green"), SerializeField, HideInInspector]
        Material greenMaterial;

        private void LateUpdate() {
            var materials = meshRenderer.materials;
            if (moneyService.Balance < 100) {
                DisableInteractive = true;
                materials[0] = grayMaterial;
            }
            else {
                DisableInteractive = false;
                materials[0] = greenMaterial;
            }
            meshRenderer.materials = materials;
        }

        /*
         * Interactメソッドは、ユーザーがゲームオブジェクトとインタラクトしたときに呼び出されます。
         * MoneyServiceから100を消費します。
         */
        public override void Interact() {
            moneyService.Consume(100);
        }
    }
}
