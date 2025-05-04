using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private AudioClip clickSound; // ← クリック音
    private AudioSource audioSource;

    void Start()
    {
        // カーソルを表示 & ロック解除
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        float time = GameManager.Instance?.FinalSurvivalTime ?? 0f;
        int totalSeconds = Mathf.FloorToInt(time);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        resultText.text = $"Time:{minutes:D2}:{seconds:D2}";
    }

    public void Retry()
    {
        Destroy(GameManager.Instance?.gameObject);
        SceneManager.LoadScene("LoadScene");

    }


    public void BackToTitle()
    {
        PlaySoundAndLoad("Title");
    }

    void PlaySoundAndLoad(string sceneName)
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
            StartCoroutine(LoadSceneAfterSound(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName); // 音が設定されていない場合は即移動
        }
    }

    System.Collections.IEnumerator LoadSceneAfterSound(string sceneName)
    {
        yield return new WaitForSecondsRealtime(0.3f); // 音が鳴るのを待つ
        SceneManager.LoadScene(sceneName);
    }
}
