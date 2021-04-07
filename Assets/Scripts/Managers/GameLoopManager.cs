using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public static GameLoopManager instance = null;
    private static List<CharController> activeChars;

    private static bool isPlayerTurn = true;


    
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        activeChars = FindObjectsOfType<CharController>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTurn();
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

    private static void UpdateTurn()
    {
        bool allDone = true;
        //iterating in reverse order to be able to safely delete inactive characters
        for (var i = activeChars.Count - 1; i >= 0; i--)
        {
            if (!activeChars[i].enabled) activeChars.RemoveAt(i);
            else if (!activeChars[i].doneTurn) allDone = false;
        }
        /*foreach (var activeChar in activeChars)
        {
            if (!activeChar.doneTurn) allDone = false;
        }*/

        if (allDone)
        {
            Debug.Log("setting player turn true");
            SetPlayerTurn(true);
            foreach (var activeChar in activeChars)
            {
                activeChar.doneTurn = false;
            }
        }
    }

}