using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class StartButton : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    private AudioSource audioSource;

    void Start()
    {
        Time.timeScale = 1f;
        audioSource = GetComponent<AudioSource>();
        var button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            PlayClickSound();
            StartCoroutine(LoadSceneWithDelay());
        });
    }

    void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    System.Collections.IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSecondsRealtime(0.3f); // ‰¹‚ª–Â‚éŠÔ‚Ì‘Ò‹@
        SceneManager.LoadScene("LoadScene");
    }
}
