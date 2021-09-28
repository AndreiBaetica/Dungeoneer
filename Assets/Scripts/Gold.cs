using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

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
        Vector3 moveUp = position;
        moveUp.y = 1.5f;
        //moveUp.z = 9;
       // moveUp.x = 1;

        GameObject prefab = Instantiate(goldIndicatorPrefab, moveUp, Quaternion.identity);
        prefab.GetComponentInChildren<TextMesh>().text = "+"+amount+" Gold";

    }
}