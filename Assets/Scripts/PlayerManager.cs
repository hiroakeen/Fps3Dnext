using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerController;
    [SerializeField] private MonoBehaviour cameraController;

    public void EnableControl()
    {
        if (playerController != null) playerController.enabled = true;
        if (cameraController != null) cameraController.enabled = true;
    }

    public void DisableControl()
    {
        if (playerController != null) playerController.enabled = false;
        if (cameraController != null) cameraController.enabled = false;
    }
}
