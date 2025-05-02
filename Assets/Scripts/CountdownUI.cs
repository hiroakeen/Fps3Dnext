using UnityEngine;
using TMPro;
using DG.Tweening;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    public void ShowCountdown(int number)
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = number.ToString();
        countdownText.color = new Color(1, 1, 1, 1);
        countdownText.transform.localScale = Vector3.one * 0.5f;

        // DoTweenアニメーション
        Sequence seq = DOTween.Sequence();
        seq.Append(countdownText.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
        seq.Join(countdownText.DOFade(1f, 0.2f));
        seq.AppendInterval(0.5f);
        seq.Append(countdownText.DOFade(0f, 0.2f));
    }

    public void ShowStart()
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = "START!";
        countdownText.color = new Color(1, 1, 1, 1);
        countdownText.transform.localScale = Vector3.one * 0.5f;

        Sequence seq = DOTween.Sequence();
        seq.Append(countdownText.transform.DOScale(1.2f, 0.4f).SetEase(Ease.OutBack));
        seq.Join(countdownText.DOFade(1f, 0.3f));
        seq.AppendInterval(0.8f);
        seq.Append(countdownText.DOFade(0f, 0.3f))
            .AppendCallback(() => countdownText.gameObject.SetActive(false));
    }
}
