using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] private Image whiteFadeImage; // ← インスペクタでアサイン

    private bool hasStarted = false;

    void Start()
    {
        if (whiteFadeImage != null)
        {
            Color c = whiteFadeImage.color;
            c.a = 0f;
            whiteFadeImage.color = c;
        }
    }

    private void Update()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            StartCoroutine(LoadSceneCountDown());
        }
    }

    IEnumerator LoadSceneCountDown()
    {
        yield return new WaitForSeconds(3f); // 最初の3秒はそのまま

        // ここからホワイトアウト1秒
        float duration = 1f;
        float timer = 0f;

        Color color = whiteFadeImage.color;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / duration);
            whiteFadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene("MainScene");
    }
}
