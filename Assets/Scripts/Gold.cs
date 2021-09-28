using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gold : MonoBehaviour
{

    [SerializeField]private Text goldText;
    public static int gold = 0;


    protected void Start()
    {
        SetGoldText(0);
    }

    public void IncrementGold(int amount)
    {
        gold += amount;
        SetGoldText(gold);
    }
    
    public void DecrementGold(int amount)
    {
        gold -= amount;
        SetGoldText(gold);
    }
    
    private void SetGoldText(int goldTotal)
    {
        goldText.text = "Gold: "+goldTotal;
    }
    
    public static void CreateUpdate(Vector3 position, int amount, GameObject goldIndicatorPrefab)
    {

        GameObject prefab = Instantiate(goldIndicatorPrefab, position, Quaternion.identity);
        prefab.GetComponentInChildren<TextMesh>().text = "+"+amount+" Gold";

    }
}