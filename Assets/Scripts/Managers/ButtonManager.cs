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
            SceneManager.LoadScene("Home");
        }

        public void PlaySavedGame()
        {
            PlayerSaveData data = SaveSystem.LoadPlayer();
            if (data != null)
            {
                GameStartManager.PlayingSavedGame = true;
                if (data.location == PlayerController.Location.Home)
                {
                    SceneManager.LoadScene("Home");
                }else if (data.location == PlayerController.Location.Dungeon)
                {
                    SceneManager.LoadScene("TestDungeon");
                }
            }
            else
            {
                GameStartManager.PlayingSavedGame = false;
                SceneManager.LoadScene("Home");
            }
        }

        public void SaveGame()
        {
            SaveSystem.SavePlayer(PlayerController.instance);
            GameStartManager.PlayingSavedGame = true;
        }
        
        public void DungeonButton()
        {
            if (SceneManager.GetActiveScene().name == "Home")
            {
                SceneManager.LoadScene("TestDungeon");
                PlayerController.instance.playerLocation = PlayerController.Location.Dungeon;
            } 
            else if (SceneManager.GetActiveScene().name == "TestDungeon")
            {
                SceneManager.LoadScene("Home");
                PlayerController.instance.playerLocation = PlayerController.Location.Home;
            }
        }
    }
}
