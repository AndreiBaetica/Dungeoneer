﻿using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Gold : MonoBehaviour
{
    [SerializeField] private GameObject goldIndicator;
    [SerializeField]private Text goldText;

    protected void Start()
    {
        SetGoldText();
    }
    public void IncrementGold(int amount, Vector3 position)
    {
        PlayerController.instance.CurrentGold += amount;
        CreateIndicator(position, 5, goldIndicator,true);
        SetGoldText();
    }

    public void DecrementGold(int amount, Vector3 position)
    {
        PlayerController.instance.CurrentGold -= amount;
        CreateIndicator(position, 5, goldIndicator,false);
        SetGoldText();
    }

    private void SetGoldText()
    {
        goldText.text = "Gold: "+PlayerController.instance.CurrentGold;
    }
    


    public static void CreateIndicator(Vector3 position, int amount, GameObject goldIndicatorPrefab, bool increment)
    {
        Vector3 moveUp = position;
        moveUp.y = 1.5f;

        GameObject prefab = Instantiate(goldIndicatorPrefab, moveUp, Quaternion.identity);
        if (increment)
        {
            prefab.GetComponentInChildren<TextMesh>().text = "+"+amount+" Gold";
        }
        else
        {
            prefab.GetComponentInChildren<TextMesh>().text = "-"+amount+" Gold";
        }

    }
}