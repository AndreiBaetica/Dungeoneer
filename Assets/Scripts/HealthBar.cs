using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public int health;

    public void SetMaxHealth(int health)
    {
        this.health = health;
        slider.maxValue = health;
        slider.value = health;

        gradient.Evaluate(1);
    }
    
    public void SetHealth(int health)
    {
        slider.value = health;
        this.health = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }


}
