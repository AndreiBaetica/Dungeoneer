using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorLevel : MonoBehaviour
{
    #region Singleton

    public static FloorLevel instance;
    
    void Awake()   
    {
        instance = this;
    }
        
    #endregion
    
    public static FloorLevel MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FloorLevel>();
            }

            return instance;
        }
    }
    
    public static Text[] pointsText;

    public void UpdateFloorText()
    {
        pointsText = FindObjectsOfType<Text>();
        for (int i = 0; i < pointsText.Length; i++)
        {
            if (pointsText[i].name == "FloorText")
            {
                pointsText[i].text = "FLOOR "+ UIManager.FinalRoomScore.ToString();
            }
        }
    }
}
