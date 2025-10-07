using Fusion;
using TMPro;
using Unity.Services.Multiplayer;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText = null;

    [SerializeField] GameLauncher gameLauncher = null;

    private int score = 0;
    private void Start()
    {
        scoreText.text = "Score:0";

        //GameLuncharのイベントを購読
        gameLauncher.OnPlayerJoined += OnPlayerJoined;
    }

    void OnPlayerJoined(NetworkRunner runner, PlayerRef player, NetworkObject spawnedObject)
    {
        // プレイヤーが参加したときの処理
        Debug.Log($"Player {player} has joined the game.");

        spawnedObject.GetComponent<PlayerAvatar>().OnItemCollectedAction += (Object, item) =>
        {
            Debug.Log($"{Object.NickName.Value} collected an item:{item.Object.Id}");

            score++;
            scoreText.text = $"Score:{score}";
        };
    }
}
