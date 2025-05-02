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
            seq.Append(loadingText.DOFade(0f, 1f)); // 1�b�����ē�����
            seq.Join(loadingText.rectTransform.DOScale(1.2f, 1f)); // �����Ɋg��
            seq.Append(loadingText.DOFade(1f, 1f)); // 1�b�����Č��ɖ߂�
            seq.Join(loadingText.rectTransform.DOScale(1f, 1f)); // �T�C�Y���߂�
        }

        Invoke(nameof(StopAnimation), 3f);
    }

    private void StopAnimation()
    {
        if (seq != null && seq.IsActive())
        {
            seq.Kill(); // �����ƒ�~
        }
    }

    private void OnDestroy()
    {
        if (seq != null && seq.IsActive())
        {
            seq.Kill(); // �V�[���j��������~
        }
    }
}
