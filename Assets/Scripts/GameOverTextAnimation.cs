using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTextAnimation : MonoBehaviour
{
    void Start()
    {
        var transformCache = transform;
        var defaultPosition = transformCache.localPosition;
        transformCache.DOLocalMove(defaultPosition, 1f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                transformCache.DOShakePosition(1.5f, 100);
            });
    }

}
