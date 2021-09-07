using System;
using UnityEngine;

public class SavedGameIndicator : MonoBehaviour
{

    public static void Create(Vector3 position, GameObject savedGameIndicatorPrefab)
    {
        Debug.Log("SomeVariable has not been assigned."+position.ToString())    ;
        Instantiate(savedGameIndicatorPrefab, position, Quaternion.identity);
    }

}