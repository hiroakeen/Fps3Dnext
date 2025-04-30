using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleAnimator : MonoBehaviour
{
    public RectTransform titleText;
    public RectTransform startButton;

    void Start()
    {
        AnimateTitle();
    }

    void AnimateTitle()
    {
        // タイトルをスケール0にしてからスタート
        titleText.localScale = Vector3.zero;
        startButton.localScale = Vector3.zero;
        startButton.GetComponent<CanvasGroup>().alpha = 0f;

        Sequence sequence = DOTween.Sequence();

        // タイトルのスケールアニメーション
        sequence.Append(titleText.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutBack))

            // 少し待ってからボタンをアニメーション
            .AppendInterval(0.3f)

            // ボタンをフェードイン＆スケールアップ
            .Append(startButton.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack))
            .Join(startButton.GetComponent<CanvasGroup>().DOFade(1f, 0.8f));
    }
}
