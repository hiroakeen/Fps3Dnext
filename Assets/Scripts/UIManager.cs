using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private GameObject gameClearText;
    [SerializeField] private TextMeshProUGUI timerText;
    private void Awake()
    {
        if (gameClearText != null)
        {
            gameClearText.SetActive(false); // �Q�[���J�n���ɔ�\���ɂ���
        }

        if (readyText != null)
        {
            readyText.gameObject.SetActive(true); // Ready�e�L�X�g�͍ŏ��ɕ\�������
        }
    }

    public void ShowGameClearText()
    {
        if (gameClearText != null)
        {
            gameClearText.SetActive(true);
        }
    }

    public void HideReadyText()
    {
        if (readyText != null)
            readyText.gameObject.SetActive(false);
    }

    public void StartReadyCountdown(System.Action onComplete)
    {
        StartCoroutine(CountdownCoroutine(onComplete));
    }

    private IEnumerator CountdownCoroutine(System.Action onComplete)
    {
        if (readyText == null)
        {
            onComplete?.Invoke();
            yield break;
        }

        string[] messages = { "Ready!", "3", "2", "1", "Go!" };

        foreach (var msg in messages)
        {
            readyText.text = msg;
            PlayPopAnimation();
            yield return new WaitForSeconds(1f);
        }

        HideReadyText();
        onComplete?.Invoke(); // �J�E���g�_�E���I����ɌĂяo��
    }

    private void PlayPopAnimation()
    {
        readyText.transform.localScale = Vector3.zero;
        readyText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void UpdateTimerUI(string formattedTime)
    {
        if (timerText != null)
        {
            timerText.text = formattedTime;
        }
    }

}
