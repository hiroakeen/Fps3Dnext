using UnityEngine;
using System;

public class TigerTracker : MonoBehaviour
{
    private Action onDestroyed;

    public void Init(Action onDestroyedCallback)
    {
        onDestroyed = onDestroyedCallback;
    }

    private void OnDestroy()
    {
        onDestroyed?.Invoke();
    }
}
