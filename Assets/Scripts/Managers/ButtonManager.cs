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
        }
        public void ExitGamePermanently()
        {
            UnityEditor.EditorApplication.isPlaying = false; //Quits game from editor
            Application.Quit(); //Quits the game if it is running properly
        }
        public void PlayNewGame()
        {
            GameStartManager.PlayingSavedGame = false;
            SceneManager.LoadScene("SampleScene");
        }

        public void PlaySavedGame()
        {
            SceneManager.LoadScene("SampleScene");
            GameStartManager.PlayingSavedGame = true;
        }
        

        public void SaveGame()
        {
            SaveSystem.SavePlayer(PlayerController.instance);
        }
    }
}
