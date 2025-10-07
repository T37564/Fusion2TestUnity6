using Fusion;
using System;
using TMPro;
using UnityEngine;

public class PlayerAvatar : NetworkBehaviour
{
    public event Action<PlayerAvatar, Item> OnItemCollectedAction = null;

    [SerializeField] public TextMeshPro text = null;

    // �v���C���[���̃l�b�g���[�N�v���p�e�B���`����
    [Networked] public NetworkString<_16> NickName { get; set; }

    [Networked] public int playerId { get; set; } = 0;

    private NetworkCharacterController characterController;

    private NetworkMecanimAnimator networkAnimator;

    public override void Spawned()
    {
        characterController = GetComponent<NetworkCharacterController>();
        networkAnimator = GetComponentInChildren<NetworkMecanimAnimator>();

        var view = GetComponent<PlayerAvatarView>();

        // �v���C���[�����e�L�X�g�ɔ��f����
        view.SetNickName(NickName.Value);

        // ���g���A�o�^�[�̌����������Ă���Ȃ�A�J�����̒Ǐ]�Ώۂɂ���
        if (HasStateAuthority)
        {
            view.MakeCameraTarget();
        }
    }

    public override void FixedUpdateNetwork()
    {
        // �ړ�
        var cameraRotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        var inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        characterController.Move(cameraRotation * inputDirection);

        // �W�����v
        if (Input.GetKey(KeyCode.Space))
        {
            characterController.Jump();
        }

        // �A�j���[�V�����i�����ł͐������ȒP�ɂ��邽�߁A���Ȃ��G�c�Ȑݒ�ɂȂ��Ă��܂��j
        var animator = networkAnimator.Animator;
        var grounded = characterController.Grounded;
        var vy = characterController.Velocity.y;
        animator.SetFloat("Speed", characterController.Velocity.magnitude);
        animator.SetBool("Jump", !grounded && vy > 4f);
        animator.SetBool("Grounded", grounded);
        animator.SetBool("FreeFall", !grounded && vy < -4f);
        animator.SetFloat("MotionSpeed", 1f);
    }

    public void OnItemCollected(Item item)
    {
        Debug.Log($"{NickName.Value}collected item:{item.Object.Id}");

        OnItemCollectedAction?.Invoke(this, item);

        // �����ɃA�C�e���擾���̃��W�b�N��ǉ��ł���
        text.text = $"{NickName.Value} collected an item: {item.Object.Id}"; // �X�R�A�̍X�V
    }

}
