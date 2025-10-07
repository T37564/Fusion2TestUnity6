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

        //GameLunchar�̃C�x���g���w��
        gameLauncher.OnPlayerJoined += OnPlayerJoined;
    }

    void OnPlayerJoined(NetworkRunner runner, PlayerRef player, NetworkObject spawnedObject)
    {
        // �v���C���[���Q�������Ƃ��̏���
        Debug.Log($"Player {player} has joined the game.");

        spawnedObject.GetComponent<PlayerAvatar>().OnItemCollectedAction += (Object, item) =>
        {
            Debug.Log($"{Object.NickName.Value} collected an item:{item.Object.Id}");

            score++;
            scoreText.text = $"Score:{score}";
        };
    }
}
