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
    public class GameManager : MonoBehaviour
    {
        
        #region Singleton

        private static GameManager instance;

        void Awake()
        {
            instance = this;
        }
        
        #endregion
        
        public Text[] pointsText;
        public Canvas[] gameUICanvas;
        public Canvas gameOverScreen;
        public Canvas gameUI;
        public Canvas pauseMenu;
        public int finalRoomScore;
        void Start()
        {
            gameUICanvas = FindObjectsOfType<Canvas>();
            for (int i = 0; i < gameUICanvas.Length; i++)
            {
                if (gameUICanvas[i].name == "GameManager")
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

            gameOverScreen.enabled = false;
            gameUI.enabled = true;
            pauseMenu.enabled = false;
        }
        
        public void EndGame()
        {
            pauseGameTime();
            gameOverScreen.enabled = true;
            gameUI.enabled = false;
            pauseMenu.enabled = false;

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


        public bool pauseGame()
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


        public bool resumeGame()
        {
            resumeGameTime();
            gameOverScreen.enabled = false;
            gameUI.enabled = true;
            pauseMenu.enabled = false;
            bool isFree = true;
            return isFree;
        }
        
        public void pauseGameTime()
        {
            Time.timeScale = 0;
        }

        public void resumeGameTime()
        {
            Time.timeScale = 1;
        }

        public bool isFrozen()
        {
            return Time.timeScale.Equals(0);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseGame();
            }

        }

    }
}