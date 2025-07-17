using HoshinoLabs.Sardinject;
using Samples.MoneyManage.Udon;
using UnityEngine;

namespace Samples.MoneyManage {
    /*
     * TheGameInstallerは、TheGameをインストールするためのコンポーネントです。
     * このコンポーネントは、TheGameのインスタンスを新しいゲームオブジェクトに登録し、
     * 必要なパラメータを渡します。
     */
    [AddComponentMenu("")]
    public class TheGameInstaller : MonoBehaviour, IInstaller {
        [SerializeField]
        Material grayMaterial;
        [SerializeField]
        Material greenMaterial;

        public void Install(ContainerBuilder builder) {
            // Colliderを持つオブジェクトのTransformを取得します
            var pushSphere = transform.GetComponentInChildren<Collider>().transform;
            // TheGameコンポーネントをエントリポイントとして追加します
            // エントリポイントなので他から参照されていなくても生成されます
            builder.RegisterEntryPoint<TheGame>(Lifetime.Cached)
                .UnderTransform(pushSphere)// TheGameをColliderと同じTransformに追加します
                .WithParameter("meshRenderer", pushSphere.GetComponent<MeshRenderer>())// TheGameにMeshRendererコンポーネントを注入します
                .WithParameter("grayMaterial", grayMaterial)// TheGameに灰色のMaterialを注入します
                .WithParameter("greenMaterial", greenMaterial);// TheGameに緑色のMaterialを注入します
        }
    }
}
