using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
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

        public void SaveGame(GameObject savedGameIndicator)
        {
            SaveSystem.SavePlayer(PlayerController.instance);
            SavedGameIndicator.Create(transform.position, savedGameIndicator);

        }
    }
}
