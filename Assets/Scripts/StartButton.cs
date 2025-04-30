using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class StartButton : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
        var button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("LoadScene");
        });
    }


}
