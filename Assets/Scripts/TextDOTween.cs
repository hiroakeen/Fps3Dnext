using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextDOTween : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    private Sequence seq;

    private void Start()
    {
        AnimateLoadingText();
    }

    private void AnimateLoadingText()
    {
        seq = DOTween.Sequence();

        for (int i = 0; i < 2; i++)
        {
            seq.Append(loadingText.DOFade(0f, 1f)); // 1•b‚©‚¯‚Ä“§–¾‚É
            seq.Join(loadingText.rectTransform.DOScale(1.2f, 1f)); // “¯Žž‚ÉŠg‘å
            seq.Append(loadingText.DOFade(1f, 1f)); // 1•b‚©‚¯‚ÄŒ³‚É–ß‚·
            seq.Join(loadingText.rectTransform.DOScale(1f, 1f)); // ƒTƒCƒY‚à–ß‚·
        }

        Invoke(nameof(StopAnimation), 3f);
    }

    private void StopAnimation()
    {
        if (seq != null && seq.IsActive())
        {
            seq.Kill(); // ‚¿‚á‚ñ‚Æ’âŽ~
        }
    }

    private void OnDestroy()
    {
        if (seq != null && seq.IsActive())
        {
            seq.Kill(); // ƒV[ƒ“”jŠüŽž‚à’âŽ~
        }
    }
}
