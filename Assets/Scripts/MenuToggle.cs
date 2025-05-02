using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    private bool isMenuOpen = false;

    private void Start()
    {
        menuPanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuPanel.SetActive(isMenuOpen);

        if (isMenuOpen)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
