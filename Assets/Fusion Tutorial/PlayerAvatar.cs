using Fusion;
using System;
using TMPro;
using UnityEngine;

public class PlayerAvatar : NetworkBehaviour
{
    public event Action<PlayerAvatar, Item> OnItemCollectedAction = null;

    [SerializeField] public TextMeshPro text = null;

    // プレイヤー名のネットワークプロパティを定義する
    [Networked] public NetworkString<_16> NickName { get; set; }

    [Networked] public int playerId { get; set; } = 0;

    private NetworkCharacterController characterController;

    private NetworkMecanimAnimator networkAnimator;

    public override void Spawned()
    {
        characterController = GetComponent<NetworkCharacterController>();
        networkAnimator = GetComponentInChildren<NetworkMecanimAnimator>();

        var view = GetComponent<PlayerAvatarView>();

        // プレイヤー名をテキストに反映する
        view.SetNickName(NickName.Value);

        // 自身がアバターの権限を持っているなら、カメラの追従対象にする
        if (HasStateAuthority)
        {
            view.MakeCameraTarget();
        }
    }

    public override void FixedUpdateNetwork()
    {
        // 移動
        var cameraRotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        var inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        characterController.Move(cameraRotation * inputDirection);

        // ジャンプ
        if (Input.GetKey(KeyCode.Space))
        {
            characterController.Jump();
        }

        // アニメーション（ここでは説明を簡単にするため、かなり大雑把な設定になっています）
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

        // ここにアイテム取得時のロジックを追加できる
        text.text = $"{NickName.Value} collected an item: {item.Object.Id}"; // スコアの更新
    }

}
