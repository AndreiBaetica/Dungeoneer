using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using Unity.Collections.LowLevel.Unsafe;

namespace Managers
{
    public class OverlayManager : MonoBehaviour
    {
        
        #region Singleton

        private static OverlayManager instance;

        void Awake()   
        {
            instance = this;
        }
        
        #endregion
        
        public static Text[] pointsText;
        public Canvas[] gameUICanvas;
        private static Canvas gameOverScreen;
        private static Canvas gameUI;
        private static Canvas pauseMenu;
        public static int FinalRoomScore;
        private static bool acceptingInput;
        void Start()
        {
            gameUICanvas = FindObjectsOfType<Canvas>();
            for (int i = 0; i < gameUICanvas.Length; i++)
            {
                if (gameUICanvas[i].name == "GameOverMenu")
                {
                    gameOverScreen = gameUICanvas[i];
                }
                if (gameUICanvas[i].name == "Canvas")
                {
                    gameUI = gameUICanvas[i];
                }
                if (gameUICanvas[i].name == "PauseMenu")
                {
                    pauseMenu = gameUICanvas[i];
                }

            }

           // gameOverScreen.enabled = false;
           // gameUI.enabled = true;
           // pauseMenu.enabled = false;
            resumeGame();
        }
        
        public static void EndGame()
        {
            pauseGameTime();
            acceptingInput = false;
            gameOverScreen.enabled = true;
            gameUI.enabled = false;
            pauseMenu.enabled = false;

           int foundTextIndex=0;
           pointsText = FindObjectsOfType<Text>();
           for (int i = 0; i < pointsText.Length; i++)
           {
               if (pointsText[i].name == "SCORE")
               {
                   pointsText[i].text = "YOU MADE IT TO DUNGEON "+ FinalRoomScore.ToString();
                   foundTextIndex = i;
               }
           }
          
           Debug.Log("Found Object: " + pointsText[foundTextIndex].text);
        }


        public static bool pauseGame()
        {
            bool isFree;
            if (isFrozen())
            {
                isFree = resumeGame();
                return isFree;
            }
            else
            {
                pauseGameTime();
                gameOverScreen.enabled = false;
                gameUI.enabled = false;
                pauseMenu.enabled = true;
                isFree = false;
                return isFree;
            }
           
        }


        public static bool resumeGame()
        {
            acceptingInput = true;
            resumeGameTime();
            gameOverScreen.enabled = false;
            gameUI.enabled = true;
            pauseMenu.enabled = false;
            bool isFree = true;
            return isFree;
        }
        
        private static void pauseGameTime()
        {
            Time.timeScale = 0;
        }

        private static void resumeGameTime()
        {
            Time.timeScale = 1;
        }

        private static bool isFrozen()
        {
            return Time.timeScale.Equals(0);
        }


        private void Update()
        {
            if (acceptingInput)
            {
                if (Input.GetKeyDown(KeyCode.Escape)) //?????????????
                {
                    pauseGame();
                }
            }

        }

    }
}