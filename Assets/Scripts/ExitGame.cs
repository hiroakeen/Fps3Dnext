using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class GameExit : MonoBehaviour
{
    public void ExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        var button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            Application.Quit();
        });

    }
}