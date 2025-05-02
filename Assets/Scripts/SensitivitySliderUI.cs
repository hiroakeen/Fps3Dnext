using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensitivitySliderUI : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI valueText;

    void Start()
    {
        sensitivitySlider.onValueChanged.AddListener(UpdateDisplay);
        UpdateDisplay(sensitivitySlider.value);
    }

    void UpdateDisplay(float value)
    {
        valueText.text = $"{value:F1}x";
    }
}
