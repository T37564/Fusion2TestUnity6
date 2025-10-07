using Fusion;
using UnityEngine;
using static Unity.Collections.Unicode;

public class Item : NetworkBehaviour    // Item�N���X��NetworkBehaviour���p�����܂�
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    public float speed = 1.0f;
    [SerializeField] private Vector3 target = Vector3.forward * 5.0f;

    // �ʒu���l�b�g���[�N�œ���
    [Networked] public Vector3 NetworkedPosition { get; set; }  // NetworkedPosition�v���p�e�B���`���܂�

    public override void Spawned()  // Start()�̑���BSpawned���\�b�h�́A�I�u�W�F�N�g���X�|�[�����ꂽ�Ƃ��ɌĂяo����܂�
    {
        // �����ʒu��ۑ�
        startPosition = transform.position;
        endPosition = startPosition + target;

        // StateAuthority�݂̂��ʒu�𐧌�
        if (Object.HasStateAuthority)
        {
            NetworkedPosition = startPosition;
        }
        else
        {
            // �N���C�A���g�͑����ɓ����ʒu�Ɉړ�
            transform.position = NetworkedPosition;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            // t��0�`1�̊Ԃ���������
            float t = Mathf.PingPong(Runner.SimulationTime * speed, 1.0f);
            // ���`��Ԃňʒu���X�V
            NetworkedPosition = Vector3.Lerp(startPosition, endPosition, t);
        }

        // ���ׂẴN���C�A���g�œ����ʒu�Ɉړ�
        transform.position = NetworkedPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Item Triggered by: {other.name}");
        // �A�C�e���ɏՓ˂����v���C���[�̃A�o�^�[���擾
        var playerAvatar = other.GetComponent<PlayerAvatar>();
        if (playerAvatar != null)
        {
            // �A�C�e�����擾�����v���C���[�ɒʒm
            playerAvatar.OnItemCollected(this);
            // �A�C�e�����폜
            Destroy(GetComponent<Collider>());
            Runner.Despawn(Object);
        }
    }

}
