using HoshinoLabs.Sardinject;
using Samples.MoneyManage.Udon;
using UnityEngine;

namespace Samples.MoneyManage {
    /*
     * TheGameInstaller�́ATheGame���C���X�g�[�����邽�߂̃R���|�[�l���g�ł��B
     * ���̃R���|�[�l���g�́ATheGame�̃C���X�^���X��V�����Q�[���I�u�W�F�N�g�ɓo�^���A
     * �K�v�ȃp�����[�^��n���܂��B
     */
    [AddComponentMenu("")]
    public class TheGameInstaller : MonoBehaviour, IInstaller {
        [SerializeField]
        Material grayMaterial;
        [SerializeField]
        Material greenMaterial;

        public void Install(ContainerBuilder builder) {
            // Collider�����I�u�W�F�N�g��Transform���擾���܂�
            var pushSphere = transform.GetComponentInChildren<Collider>().transform;
            // TheGame�R���|�[�l���g���G���g���|�C���g�Ƃ��Ēǉ����܂�
            // �G���g���|�C���g�Ȃ̂ő�����Q�Ƃ���Ă��Ȃ��Ă���������܂�
            builder.RegisterEntryPoint<TheGame>(Lifetime.Cached)
                .UnderTransform(pushSphere)// TheGame��Collider�Ɠ���Transform�ɒǉ����܂�
                .WithParameter("meshRenderer", pushSphere.GetComponent<MeshRenderer>())// TheGame��MeshRenderer�R���|�[�l���g�𒍓����܂�
                .WithParameter("grayMaterial", grayMaterial)// TheGame�ɊD�F��Material�𒍓����܂�
                .WithParameter("greenMaterial", greenMaterial);// TheGame�ɗΐF��Material�𒍓����܂�
        }
    }
}
