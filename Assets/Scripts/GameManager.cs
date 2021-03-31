using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CharController[] activeChars;



    
    // Start is called before the first frame update
    void Start()
    {
        activeChars = FindObjectsOfType<CharController>();
        //StartCoroutine(GameLoop());

    }

    // Update is called once per frame
    void Update()
    {
        GameLoopTest();
    }

    private IEnumerator GameLoop()
    {
        bool isFree = false;

        while (true)
        {
            foreach (var activeChar in activeChars)
            {
                //maybe better to remove from array entirely once script gets disabled
                //TODO: check for performance difference ^
                if (!activeChar.enabled) continue;
                isFree = activeChar.Action();
                /*while (isFree)
                {
                    isFree = activeChar.Action();
                }*/
            }
            yield return new WaitForSeconds(1f);
        }
    }
    
    private void GameLoopTest()
    {
        bool isFree = false;

            foreach (var activeChar in activeChars)
            {
                //maybe better to remove from array entirely once script gets disabled
                //TODO: check for performance difference ^
                if (!activeChar.enabled) continue;
                isFree = activeChar.Action();
                /*while (isFree)
                {
                    isFree = activeChar.Action();
                }*/
            }
    }
}
