using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    //private static CharController[] activeChars;

    private static bool isPlayerTurn = true;


    
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("playerturn" + isPlayerTurn);
    }

    public static bool GetPlayerTurn()
    {
        return isPlayerTurn;
    }

    public static void SetPlayerTurn(bool playerTurn)
    {
        isPlayerTurn = playerTurn;
    }

}
