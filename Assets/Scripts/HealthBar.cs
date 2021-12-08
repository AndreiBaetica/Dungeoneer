using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public GameObject hpTextObject;
    public static Text hpTexts;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        gradient.Evaluate(1);

        hpTextObject = GameObject.Find("HP Number");
        hpTexts = hpTextObject.GetComponent<Text>();
        hpTexts.text = health.ToString();
    }
    
    public void SetHealth(int health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
        
        hpTextObject = GameObject.Find("HP Number");
        hpTexts = hpTextObject.GetComponent<Text>();
        hpTexts.text = health.ToString();
    }
    
}
