using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{

    public Slider slider;
    public GameObject manaTextObject;
    public static Text manaTexts;


    public void SetMaxMana(int mana)
    {
        slider.maxValue = mana;
        slider.value = mana;
        
        manaTextObject = GameObject.Find("Mana Number");
        manaTexts = manaTextObject.GetComponent<Text>();
        manaTexts.text = mana.ToString();
    }
    
    public void SetMana(int mana)
    {
        slider.value = mana;
        
        manaTextObject = GameObject.Find("Mana Number");
        manaTexts = manaTextObject.GetComponent<Text>();
        manaTexts.text = mana.ToString();
    }
    
}
