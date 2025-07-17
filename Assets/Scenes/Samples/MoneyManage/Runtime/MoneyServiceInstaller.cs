using HoshinoLabs.Sardinject;
using Samples.MoneyManage.Udon;
using UnityEngine;

namespace Samples.MoneyManage {
    /*
     * MoneyServiceInstaller�́AMoneyService���C���X�g�[�����邽�߂̃R���|�[�l���g�ł��B
     * ���̃R���|�[�l���g�́AMoneyService�̃C���X�^���X��V�����Q�[���I�u�W�F�N�g�ɓo�^���A
     * ���[�g���p�����[�^�Ƃ��ēn���܂��B
     */
    [AddComponentMenu("")]
    public class MoneyServiceInstaller : MonoBehaviour, IInstaller {
        [SerializeField]
        int rate = 1;

        public void Install(ContainerBuilder builder) {
            // MoneyService�R���|�[�l���g��V�K�I�u�W�F�N�g�������Ēǉ����܂�
            builder.RegisterComponentOnNewGameObject<MoneyService>(Lifetime.Cached)// �����C���X�^���X���g���܂킷���߂�Lifetime.Cached���w��
                .WithParameter("rate", rate);// MoneyService��rate�𒍓����܂�
        }
    }
}
