using UnityEngine;
using UnityEngine.SceneManagement;
namespace Managers
{

    public class ButtonManager : MonoBehaviour {
        int n;
        public void OnButtonPress(){
            n++;
            Debug.Log("Button clicked " + n + " times.");
        }
        public void RestartGame()
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }

        public void ExitGame()
        {
            SceneManager.UnloadSceneAsync("SampleScene");
            SceneManager.LoadScene("MainMenu");
        }

        public void PlayNewGame()
        {
            SceneManager.UnloadSceneAsync("MainMenu");
            SceneManager.LoadScene("SampleScene");
        }
    }
}
