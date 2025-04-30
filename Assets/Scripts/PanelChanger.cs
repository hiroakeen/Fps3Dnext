using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Canvas))]
public class PanelChanger : MonoBehaviour
{
    public Image fadePanel; // �ŏ��͍��̃p�l��
    public string nextSceneName = "MainScene";
    public float fadeDuration = 1.0f; // �t�F�[�h�ɂ����鎞��

    private void Start()
    {
        Time.timeScale = 1f; // ���[�h�V�[�����n�܂�����Q�[�����ԃ��Z�b�g
        StartCoroutine(LoadingSequence());
    }

    private IEnumerator LoadingSequence()
    {
        // �ŏ��͍�
        fadePanel.color = Color.black;

        // 3�b�҂�
        yield return new WaitForSeconds(3f);

        // �����甒�փt�F�[�h
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

        // �t�F�[�h������A����ɏ����҂��Ă���V�[���J��
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(nextSceneName);
    }
}