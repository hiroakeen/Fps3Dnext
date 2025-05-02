using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform titleText;
    [SerializeField] private RectTransform startButton;
    [SerializeField] private CanvasGroup startButtonCanvasGroup;

    void Start()
    {
        AnimateUI();
    }

    void AnimateUI()
    {
        titleText.localScale = Vector3.zero;

        startButton.localScale = Vector3.zero;
        startButtonCanvasGroup.alpha = 1f;
        startButtonCanvasGroup.interactable = false;
        startButtonCanvasGroup.blocksRaycasts = false;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(titleText.DOScale(1f, 1.0f).SetEase(Ease.OutBack))
                .AppendInterval(0.3f)
                .Append(startButton.DOScale(1f, 0.8f).SetEase(Ease.OutBack))
                .Join(startButtonCanvasGroup.DOFade(1f, 0.8f))
                .AppendCallback(() =>
                {
                    startButtonCanvasGroup.interactable = true;
                    startButtonCanvasGroup.blocksRaycasts = true;
                });
    }
}
