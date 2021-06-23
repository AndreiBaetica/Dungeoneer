using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public Text pointsText;

        void EndGame(int score)
        {
            Debug.Log("GAME OVER: " + score.ToString() );
        }
        

        public void Setup(int score)
        {
            gameObject.SetActive(true);

            pointsText.text = "YOU MADE IT TO DUNGEON "+ score.ToString() +"and" + SceneManager.sceneCountInBuildSettings;
        }

        public void RestartButton()
        {
            Destroy(this.gameObject);
            SceneManager.LoadScene("Scences/SampleScene");
            Console.WriteLine(SceneManager.sceneCountInBuildSettings);
        }

        public void ExitButton()
        {
            //Destroy(this.gameObject);
            SceneManager.LoadScene("Scenes/SampleScene");
            Console.WriteLine(SceneManager.sceneCountInBuildSettings);

        }
    }
}