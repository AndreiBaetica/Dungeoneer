using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
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
            SceneManager.LoadScene("MainMenu");
        }

        public void PlayNewGame()
        {
            SceneManager.LoadScene("SampleScene");
        }

        public void SaveGame()
        {

            GameObject sel = EventSystem.current.currentSelectedGameObject;            
            n++;
            Debug.Log("Button clicked " + n + " times.");
            SaveSystem.SavePlayer(PlayerController.instance);
        }
    }
}
