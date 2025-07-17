using HoshinoLabs.Sardinject.Udon;
using TMPro;
using UdonSharp;
using UnityEngine;

namespace Samples.MoneyManage.Udon {
    /*
     * BalanceDisplayは、MoneyServiceの残高を表示するためのシンプルなコンポーネントです。
     * MoneyServiceから残高を取得し、TextMeshProUGUIコンポーネントに表示します。
     */
    [AddComponentMenu("")]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class BalanceDisplay : UdonSharpBehaviour {
        [Inject, SerializeField, HideInInspector]
        MoneyService moneyService;
        [Inject, SerializeField, HideInInspector]
        TextMeshProUGUI balanceText;

        private void LateUpdate() {
            balanceText.text = $"Balance: {moneyService.Balance}";
        }
    }
}
