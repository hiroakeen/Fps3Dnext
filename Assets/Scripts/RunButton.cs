using UnityEngine;
using UnityEngine.EventSystems;

public class RunButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool IsPressed { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;
    }
}
