using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ClearSceneController : MonoBehaviour
{
    public TextMeshProUGUI gameClearText;
    public TextMeshProUGUI clearTimeText;
    public TextMeshProUGUI bestTimeText;
    public Button retryButton;
    public Button exitButton;

    private void Start()
    {
        float finalTime = GameManager.Instance.GetFinalTime();
        // �ŏ��Ƀ{�^����\��
        retryButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

        Sequence clearSequence = DOTween.Sequence();

        // �Q�[���N���A�e�L�X�g�ӂ���Əo��
        gameClearText.transform.localScale = Vector3.zero;
        clearSequence.Append(gameClearText.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack));

        // �^�C���\��
        clearSequence.AppendInterval(0.5f);
        clearSequence.AppendCallback(ShowClearTime);

        // �{�^���o��
        clearSequence.AppendInterval(1f);
        clearSequence.AppendCallback(ShowButtons);
    }



    void ShowButtons()
    {
        retryButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);

        retryButton.transform.localScale = Vector3.zero;
        exitButton.transform.localScale = Vector3.zero;

        retryButton.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        exitButton.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    // �{�^���̃C�x���g�p
    public void OnRetryButton()
    {
        SceneManager.LoadScene("LoadScene"); // ���C���V�[���ɖ߂�
    }

    public void OnExitButton()
    {
        SceneManager.LoadScene("TitleScene"); // �^�C�g���V�[���ɖ߂�
    }


    void ShowClearTime()
    {
        // �N���A�^�C���\��
        float finalTime = GameManager.Instance.GetFinalTime();
        int minutes = Mathf.FloorToInt(finalTime / 60F);
        int seconds = Mathf.FloorToInt(finalTime % 60F);
        int milliseconds = Mathf.FloorToInt((finalTime * 100F) % 100F);
        clearTimeText.text = $"Time: {minutes:00}:{seconds:00}.{milliseconds:00}";

        // �x�X�g�^�C�����\��
        float bestTime = GameManager.Instance.GetBestTime();
        if (bestTime != float.MaxValue) // �L�^������ꍇ�̂�
        {
            int bestMinutes = Mathf.FloorToInt(bestTime / 60F);
            int bestSeconds = Mathf.FloorToInt(bestTime % 60F);
            int bestMilliseconds = Mathf.FloorToInt((bestTime * 100F) % 100F);
            bestTimeText.text = $"Best: {bestMinutes:00}:{bestSeconds:00}.{bestMilliseconds:00}";
        }
        else
        {
            bestTimeText.text = "Best: ---";
        }

        // �����e�L�X�g�Ƀt�F�[�h�A�j������Ȃ炱����DoTween
    }

}

