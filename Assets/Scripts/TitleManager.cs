using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void SelectPC()
    {
        PlayerPrefs.SetString("DeviceMode", "PC");
        SceneManager.LoadScene("LoadScene");
    }

    public void SelectMobile()
    {
        PlayerPrefs.SetString("DeviceMode", "Mobile");
        SceneManager.LoadScene("LoadScene");
    }
}
