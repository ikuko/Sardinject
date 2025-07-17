using HoshinoLabs.Sardinject;
using Samples.MoneyManage.Udon;
using TMPro;
using UnityEngine;

namespace Samples.MoneyManage {
    /*
     * BalanceDisplayInstallerは、BalanceDisplayをインストールするためのコンポーネントです。
     * このコンポーネントは、BalanceDisplayのインスタンスを新しいゲームオブジェクトに登録し、
     * balanceTextをパラメータとして渡します。
     */
    [AddComponentMenu("")]
    public class BalanceDisplayInstaller : MonoBehaviour, IInstaller {
        public void Install(ContainerBuilder builder) {
            // BalanceDisplayコンポーネントをエントリポイントとして追加します
            // エントリポイントなので他から参照されていなくても生成されます
            builder.RegisterEntryPoint<BalanceDisplay>(Lifetime.Cached)
                .UnderTransform(transform)// BalanceDisplayをこのTransformに追加します
                .WithParameter("balanceText", transform.GetComponentInChildren<TextMeshProUGUI>());// BalanceDisplayにTextMeshProUGUIコンポーネントを注入します
        }
    }
}
