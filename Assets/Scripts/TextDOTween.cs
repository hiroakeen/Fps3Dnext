using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextDOTween : MonoBehaviour
{
    public Text loadingText;
    private Sequence seq;

    private void Start()
    {
        AnimateLoadingText();
    }

    private void AnimateLoadingText()
    {
        seq = DOTween.Sequence(); // ‚±‚±‚Å³‚µ‚¢seqì¬

        seq.Append(loadingText.DOFade(0f, 1f)); // 1•b‚©‚¯‚Ä“§–¾‚É
        seq.Join(loadingText.rectTransform.DOScale(1.2f, 1f)); // “¯‚ÉŠg‘å
        seq.Append(loadingText.DOFade(1f, 1f)); // 1•b‚©‚¯‚ÄŒ³‚É–ß‚·
        seq.Join(loadingText.rectTransform.DOScale(1f, 1f)); // ƒTƒCƒY‚à–ß‚·


        // 4•bŒã‚ÉTween‚ğ~‚ß‚é
        Invoke(nameof(StopAnimation), 3.6f);
    }

    private void StopAnimation()
    {
        if (seq != null && seq.IsActive())
        {
            seq.Kill(); // ‚¿‚á‚ñ‚Æ’â~
        }
    }

    private void OnDestroy()
    {
        if (seq != null && seq.IsActive())
        {
            seq.Kill(); // ƒV[ƒ“”jŠü‚à’â~
        }
    }
}
