using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Canvas))]
public class PanelChanger : MonoBehaviour
{
    public Image fadePanel; // 最初は黒のパネル
    public string nextSceneName = "MainScene";
    public float fadeDuration = 1.0f; // フェードにかかる時間

    private void Start()
    {
        Time.timeScale = 1f; // ロードシーンが始まったらゲーム時間リセット
        StartCoroutine(LoadingSequence());
    }

    private IEnumerator LoadingSequence()
    {
        // 最初は黒
        fadePanel.color = Color.black;

        // 3秒待つ
        yield return new WaitForSeconds(3f);

        // 黒から白へフェード
        float elapsed = 0f;
        Color startColor = Color.black;
        Color endColor = Color.white;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            fadePanel.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        // フェード完了後、さらに少し待ってからシーン遷移
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(nextSceneName);
    }
}