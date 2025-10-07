using Unity.Cinemachine;
using UnityEngine;
using TMPro;

public class PlayerAvatarView : MonoBehaviour
{
    [SerializeField]
    private CinemachineCamera cinemachineCamera;

    [SerializeField] private TextMeshPro nameLabel;

    public void MakeCameraTarget()
    {
        // CinemachineCamera�̗D��x���グ�āA�J�����̒Ǐ]�Ώۂɂ���
        cinemachineCamera.Priority.Value = 100;
    }


    public void SetNickName(string nickName)
    {
        nameLabel.text = nickName;
    }

    private void LateUpdate()
    {
        // �v���C���[���̃e�L�X�g���A�r���{�[�h�i��ɃJ�������ʌ����j�ɂ���
        nameLabel.transform.rotation = Camera.main.transform.rotation;
    }
}