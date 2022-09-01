using UnityEngine;
using UnityEngine.UI;

public class BarSlider : MonoBehaviour
{
    public Slider slider;

    public void Set_MaxBarValue(int barValue)
    {
        slider.maxValue = barValue;
        slider.value = barValue;
    }

    public void Set_BarValue(int barValue)
    {
        slider.value = barValue;
    }
}
