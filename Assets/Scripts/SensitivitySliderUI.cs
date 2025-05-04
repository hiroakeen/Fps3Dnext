using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensitivitySliderUI : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private CameraLookController cameraLookController; // ä¥ìxîΩâf

    [SerializeField] private float multiplier = 2.0f; // ïœâªÇÃã≠í≤åWêî

    void Start()
    {
        sensitivitySlider.onValueChanged.AddListener(UpdateDisplay);
        UpdateDisplay(sensitivitySlider.value);
    }

    void UpdateDisplay(float value)
    {
        float displayValue = value * multiplier;
        valueText.text = $"{displayValue:F1}x";

        if (cameraLookController != null)
        {
            cameraLookController.SetMouseSensitivity(displayValue);
        }
    }
}
