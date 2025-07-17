using HoshinoLabs.Sardinject;
using Samples.MoneyManage.Udon;
using TMPro;
using UnityEngine;

namespace Samples.MoneyManage {
    /*
     * BalanceDisplayInstaller�́ABalanceDisplay���C���X�g�[�����邽�߂̃R���|�[�l���g�ł��B
     * ���̃R���|�[�l���g�́ABalanceDisplay�̃C���X�^���X��V�����Q�[���I�u�W�F�N�g�ɓo�^���A
     * balanceText���p�����[�^�Ƃ��ēn���܂��B
     */
    [AddComponentMenu("")]
    public class BalanceDisplayInstaller : MonoBehaviour, IInstaller {
        public void Install(ContainerBuilder builder) {
            // BalanceDisplay�R���|�[�l���g���G���g���|�C���g�Ƃ��Ēǉ����܂�
            // �G���g���|�C���g�Ȃ̂ő�����Q�Ƃ���Ă��Ȃ��Ă���������܂�
            builder.RegisterEntryPoint<BalanceDisplay>(Lifetime.Cached)
                .UnderTransform(transform)// BalanceDisplay������Transform�ɒǉ����܂�
                .WithParameter("balanceText", transform.GetComponentInChildren<TextMeshProUGUI>());// BalanceDisplay��TextMeshProUGUI�R���|�[�l���g�𒍓����܂�
        }
    }
}
