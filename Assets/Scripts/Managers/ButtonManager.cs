using System;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Managers
{

    public class ButtonManager : MonoBehaviour {
        public void RestartGame()
        {
            GameStartManager.PlayingSavedGame = false;
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }

        public void ExitGame()
        {
            SceneManager.LoadScene("MainMenu");
            PlayerController.instance.playerLocation = PlayerController.Location.Menu;

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
            SceneManager.LoadScene("SampleScene");
            PlayerController.instance.playerLocation = PlayerController.Location.Home;
        }

        public void PlaySavedGame()
        {
            SceneManager.LoadScene("SampleScene");
            if (PlayerController.instance.playerLocation == PlayerController.Location.Home)
            {
                SceneManager.LoadScene("SampleScene");
            }else if (PlayerController.instance.playerLocation == PlayerController.Location.Dungeon)
            {
                SceneManager.LoadScene("TestDungeon");
            }
            GameStartManager.PlayingSavedGame = true;
        }

        public void SaveGame()
        {
            SaveSystem.SavePlayer(PlayerController.instance);
            GameStartManager.PlayingSavedGame = true;
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
