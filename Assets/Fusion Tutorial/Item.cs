using Fusion;
using UnityEngine;
using static Unity.Collections.Unicode;

public class Item : NetworkBehaviour    // ItemクラスはNetworkBehaviourを継承します
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    public float speed = 1.0f;
    [SerializeField] private Vector3 target = Vector3.forward * 5.0f;

    // 位置をネットワークで同期
    [Networked] public Vector3 NetworkedPosition { get; set; }  // NetworkedPositionプロパティを定義します

    public override void Spawned()  // Start()の代わり。Spawnedメソッドは、オブジェクトがスポーンされたときに呼び出されます
    {
        // 初期位置を保存
        startPosition = transform.position;
        endPosition = startPosition + target;

        // StateAuthorityのみが位置を制御
        if (Object.HasStateAuthority)
        {
            NetworkedPosition = startPosition;
        }
        else
        {
            // クライアントは即座に同期位置に移動
            transform.position = NetworkedPosition;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            // tは0～1の間を往復する
            float t = Mathf.PingPong(Runner.SimulationTime * speed, 1.0f);
            // 線形補間で位置を更新
            NetworkedPosition = Vector3.Lerp(startPosition, endPosition, t);
        }

        // すべてのクライアントで同期位置に移動
        transform.position = NetworkedPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Item Triggered by: {other.name}");
        // アイテムに衝突したプレイヤーのアバターを取得
        var playerAvatar = other.GetComponent<PlayerAvatar>();
        if (playerAvatar != null)
        {
            // アイテムを取得したプレイヤーに通知
            playerAvatar.OnItemCollected(this);
            // アイテムを削除
            Destroy(GetComponent<Collider>());
            Runner.Despawn(Object);
        }
    }

}
