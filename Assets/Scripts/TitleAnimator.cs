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
        // �^�C�g�����X�P�[��0�ɂ��Ă���X�^�[�g
        titleText.localScale = Vector3.zero;
        startButton.localScale = Vector3.zero;
        startButton.GetComponent<CanvasGroup>().alpha = 0f;

        Sequence sequence = DOTween.Sequence();

        // �^�C�g���̃X�P�[���A�j���[�V����
        sequence.Append(titleText.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutBack))

            // �����҂��Ă���{�^�����A�j���[�V����
            .AppendInterval(0.3f)

            // �{�^�����t�F�[�h�C�����X�P�[���A�b�v
            .Append(startButton.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack))
            .Join(startButton.GetComponent<CanvasGroup>().DOFade(1f, 0.8f));
    }
}
