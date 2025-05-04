using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] private Image whiteFadeImage; // �� �C���X�y�N�^�ŃA�T�C��

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
        yield return new WaitForSeconds(3f); // �ŏ���3�b�͂��̂܂�

        // ��������z���C�g�A�E�g1�b
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
