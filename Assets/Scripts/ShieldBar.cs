using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public GameObject shieldTextObject;
    public static Text shieldTexts;

    public void SetMaxShield(int shield)
    {
        slider.maxValue = shield;
        slider.value = shield;

        gradient.Evaluate(1);
        
        shieldTextObject = GameObject.Find("Shield Number");
        shieldTexts = shieldTextObject.GetComponent<Text>();
        shieldTexts.text = shield.ToString();
    }
    
    public void SetShield(int shield)
    {
        slider.value = shield;

        fill.color = gradient.Evaluate(slider.normalizedValue);
        
        shieldTextObject = GameObject.Find("Shield Number");
        shieldTexts = shieldTextObject.GetComponent<Text>();
        shieldTexts.text = shield.ToString();
    }
    
}
