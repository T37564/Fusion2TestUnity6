using Unity.Cinemachine;
using UnityEngine;

public class PlayerAvatarView : MonoBehaviour
{
    [SerializeField]
    private CinemachineCamera cinemachineCamera;

    public void MakeCameraTarget()
    {
        // CinemachineCamera‚Ì—Dæ“x‚ğã‚°‚ÄAƒJƒƒ‰‚Ì’Ç]‘ÎÛ‚É‚·‚é
        cinemachineCamera.Priority.Value = 100;
    }
}