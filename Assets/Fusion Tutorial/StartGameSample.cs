using Fusion;
using UnityEngine;

public class StartGameSample : MonoBehaviour
{
    [SerializeField]
    private NetworkRunner networkRunnerPrefab;

    private async void Start()
    {
        // NetworkRunner�𐶐�����
        var networkRunner = Instantiate(networkRunnerPrefab);
        // ���L���[�h�̃Z�b�V�����ɎQ������
        var result = await networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared
        });
        // ���ʂ��R���\�[���ɏo�͂���
        Debug.Log(result);
    }
}