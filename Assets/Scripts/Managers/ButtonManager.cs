using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Managers
{

    public class ButtonManager : MonoBehaviour {
        
        #region Singleton

        public static ButtonManager instance;
    
        void Awake()   
        {
            instance = this;
        }
        
        #endregion
        
        public static ButtonManager MyInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ButtonManager>();
                }

                return instance;
            }
        }

        private static Canvas[] canvasArray;
        
        public void RestartGame()
        {
            GameStartManager.PlayingSavedGame = false;
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }

        public void ExitGame()
        {
            SceneManager.LoadScene("MainMenu");
        }
        public void ExitGamePermanently()
        {
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; //Quits game from editor
            #endif
            Application.Quit(); //Quits the game if it is running properly
            
        }
        public void PlayNewGame()
        {
            GameStartManager.PlayingSavedGame = false;
            SaveSystem.DeleteSave();
            SceneManager.LoadScene("SampleScene");
        }

        public void PlaySavedGame()
        {
            PlayerSaveData data = SaveSystem.LoadPlayer();
            if (data != null)
            {
                GameStartManager.PlayingSavedGame = true;
                if (data.location == PlayerController.Location.Home)
                {
                    SceneManager.LoadScene("SampleScene");
                }else if (data.location == PlayerController.Location.Dungeon)
                {
                    SceneManager.LoadScene("TestDungeon");
                }
            }
            else
            {
                GameStartManager.PlayingSavedGame = false;
                SceneManager.LoadScene("SampleScene");
            }
        }

        public void SaveGame()
        {
            SaveSystem.SavePlayer(PlayerController.instance);
            GameStartManager.PlayingSavedGame = true;
        }

        public void OpenControls()
        {
            canvasArray = FindObjectsOfType<Canvas>();
            foreach (var t in canvasArray)
            {
                if (t.name == "ControlCanvas")
                {
                    t.enabled = true;
                    break;
                }
            }
        }

        public void CloseControls()
        {
            canvasArray = FindObjectsOfType<Canvas>();
            foreach (var t in canvasArray)
            {
                if (t.name == "ControlCanvas")
                {
                    t.enabled = false;
                    break;
                }
            }        
        }
        
        public void DungeonButton()
        {
            if (SceneManager.GetActiveScene().name == "SampleScene")
            {
                SceneManager.LoadScene("TestDungeon");
                PlayerController.instance.playerLocation = PlayerController.Location.Dungeon;
            }else if (SceneManager.GetActiveScene().name == "TestDungeon")
            {
                SceneManager.LoadScene("SampleScene");
                PlayerController.instance.playerLocation = PlayerController.Location.Home;
            }
        }
    }
}
