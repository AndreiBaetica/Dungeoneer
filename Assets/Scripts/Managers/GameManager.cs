using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Unity.Collections.LowLevel.Unsafe;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        
        #region Singleton

        public static GameManager instance;

        void Awake()
        {
            instance = this;
        }
        
        #endregion
        
        public Text[] pointsText;
        public Canvas[] gameUICanvas;
        public Canvas gameOverScreen;
        public Canvas gameUI;
        public int finalRoomScore;
        void Start()
        {
            gameUICanvas = FindObjectsOfType<Canvas>();
            for (int i = 0; i < gameUICanvas.Length; i++)
            {
                if (gameUICanvas[i].name == "GameManager")
                {
                    gameOverScreen = gameUICanvas[i];
                    gameUI = gameUICanvas[i + 1];
                    Debug.Log(gameUICanvas[i].name + " :canvas0 & canvas1: " + gameUICanvas[i + 1].name);
                    Debug.Log(gameOverScreen.name + " :canvas0 & canvas1: " + gameUI.name);
                }
            }

            gameOverScreen.enabled = false;
            gameUI.enabled = true;
        }
        
        public void EndGame()
        {
            pauseGameTime();
            gameOverScreen.enabled = true;
            gameUI.enabled = false;

           // Debug.Log("GAME OVER: " + finalRoomScore.ToString() );
           int foundTextIndex=0;
           pointsText = FindObjectsOfType<Text>();
           for (int i = 0; i < pointsText.Length; i++)
           {
               if (pointsText[i].name == "SCORE")
               {
                   pointsText[i].text = "YOU MADE IT TO DUNGEON "+ finalRoomScore.ToString();
                   foundTextIndex = i;
               }
           }
          
           Debug.Log("Found Object: " + pointsText[foundTextIndex].text);
        }

        public void pauseGameTime()
        {
            Time.timeScale = 0;
        }

        public void resumeGameTime()
        {
            Time.timeScale = 1;
        }

    }
}