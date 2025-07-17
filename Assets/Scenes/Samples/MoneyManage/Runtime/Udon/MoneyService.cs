using HoshinoLabs.Sardinject.Udon;
using UdonSharp;
using UnityEngine;

namespace Samples.MoneyManage.Udon {
    /*
     * MoneyServiceは、時間の経過とともにお金を稼ぐことをシミュレートするシンプルなサービスです。
     * お金の消費を可能にし、残高を追跡します。
     */
    [AddComponentMenu("")]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class MoneyService : UdonSharpBehaviour {
        [Inject, SerializeField, HideInInspector]
        int rate;

        int balance;
        float elapsed;

        public int Balance => balance;

        private void LateUpdate() {
            elapsed += Time.deltaTime;
            if (0.1f <= elapsed) {
                // Simulate earning money every second
                balance += rate;
                elapsed -= 0.1f;
            }
        }

        /*
         * Consumeメソッドは、指定された金額を消費します。
         * 残高が不足している場合は警告を出力します。
         */
        public void Consume(int amount) {
            if (amount < 0) {
                return;
            }
            if (balance < amount) {
                Debug.LogWarning($"Insufficient balance: {balance} to consume {amount}");
                return;
            }
            balance -= amount;
            Debug.Log($"Consumed {amount}, new balance: {balance}");
        }
    }
}
