using UnityEngine;
using UnityEngine.UI;
//Codecodile (2023) How To Create a HealthSystem in Unity: Part 1. [Online] Available at: https://www.youtube.com/watch?v=yQers6__cLc (Accessed: 14 October 2025
public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;

    public void SetSlider(float amount)
    {
        healthSlider.value = amount;
    }

    public void SetSliderMax(float amount)
    {
        healthSlider.maxValue = amount;
        SetSlider(amount);
    }
}
