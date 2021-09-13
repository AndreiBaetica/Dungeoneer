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
            GameStartManager.PlayingSavedGame = false;
            SceneManager.LoadScene("SampleScene");
        }

        public void PlaySavedGame()
        {
            SceneManager.LoadScene("SampleScene");
            GameStartManager.PlayingSavedGame = true;
            Debug.Log("Teeeessssststtt");
           // PlayerSaveData data = SaveSystem.LoadPlayer();
           // PlayerController playerInstance = new PlayerController();
           // if (playerInstance != null)
          //  {
            //    Debug.Log("player:" + playerInstance.transform.position);

               // data.ApplyPlayerSavedData(playerInstance);
          //  }
        }
        

        public void SaveGame(GameObject savedGameIndicator)
        {

            PlayerController.instance.saved = true;
            SaveSystem.SavePlayer(PlayerManager.instance);
            Debug.Log("Player controller data pos x:"+PlayerController.instance.transform.position.x+" y:"
                      +PlayerController.instance.transform.position.y+" z:"+PlayerController.instance.transform.position.z
                      +" currenthealth:"+PlayerController.instance.currentHealth+" mana:"+PlayerManager.instance.currentMana);

            SavedGameIndicator.Create(transform.position, savedGameIndicator);

        }
    }
}
