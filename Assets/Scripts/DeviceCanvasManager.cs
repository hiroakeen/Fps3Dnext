using UnityEngine;

public class DeviceCanvasManager : MonoBehaviour
{
    public GameObject pcCanvas;
    public GameObject mobileCanvas;

    void Start()
    {
        string deviceMode = PlayerPrefs.GetString("DeviceMode", "PC");

        if (deviceMode == "Mobile")
        {
            pcCanvas.SetActive(false);
            mobileCanvas.SetActive(true);
        }
        else
        {
            pcCanvas.SetActive(true);
            mobileCanvas.SetActive(false);
        }
    }
}
