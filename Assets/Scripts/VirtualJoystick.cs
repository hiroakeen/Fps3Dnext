using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform joystickBackground;
    [SerializeField] private RectTransform joystickHandle;

    public Vector2 InputDirection { get; private set; } = Vector2.zero;

    private Vector2 centerPosition;

    void Start()
    {
        if (joystickBackground != null)
            centerPosition = joystickBackground.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground, eventData.position, eventData.pressEventCamera, out position))
        {
            float width = joystickBackground.sizeDelta.x;
            float height = joystickBackground.sizeDelta.y;

            position.x /= width;
            position.y /= height;

            InputDirection = new Vector2(position.x * 2, position.y * 2);
            InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

            joystickHandle.anchoredPosition = new Vector2(
                InputDirection.x * (width / 2),
                InputDirection.y * (height / 2)
            );
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputDirection = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }

    public void UpdateVisual(Vector2 direction)
    {
        // 外部入力（マウス/キーボード）からの視覚反応用
        direction = Vector2.ClampMagnitude(direction, 1f);
        float width = joystickBackground.sizeDelta.x;
        float height = joystickBackground.sizeDelta.y;

        joystickHandle.anchoredPosition = new Vector2(
            direction.x * (width / 2),
            direction.y * (height / 2)
        );
    }
}
