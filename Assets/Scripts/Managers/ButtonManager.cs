using UnityEngine;
using UnityEngine.SceneManagement;
namespace Managers
{

    public class ButtonManager : MonoBehaviour {
        public void RestartGame()
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }

        public void ExitGame()
        {
            SceneManager.LoadScene("MainMenu");
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
        

        public void SaveGame(GameObject savedGameIndicator)
        {
            SaveSystem.SavePlayer(PlayerController.instance);
            SavedGameIndicator.Create(transform.position, savedGameIndicator);
        }
    }
}
