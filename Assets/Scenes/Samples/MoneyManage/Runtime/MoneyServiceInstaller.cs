using HoshinoLabs.Sardinject;
using Samples.MoneyManage.Udon;
using UnityEngine;

namespace Samples.MoneyManage {
    /*
     * MoneyServiceInstallerは、MoneyServiceをインストールするためのコンポーネントです。
     * このコンポーネントは、MoneyServiceのインスタンスを新しいゲームオブジェクトに登録し、
     * レートをパラメータとして渡します。
     */
    [AddComponentMenu("")]
    public class MoneyServiceInstaller : MonoBehaviour, IInstaller {
        [SerializeField]
        int rate = 1;

        public void Install(ContainerBuilder builder) {
            // MoneyServiceコンポーネントを新規オブジェクト生成して追加します
            builder.RegisterComponentOnNewGameObject<MoneyService>(Lifetime.Cached)// 同じインスタンスを使いまわすためにLifetime.Cachedを指定
                .WithParameter("rate", rate);// MoneyServiceにrateを注入します
        }
    }
}
