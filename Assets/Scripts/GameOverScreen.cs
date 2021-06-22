using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
    {
        public Text pointsText;

        public void Setup(int score)
        {
            gameObject.SetActive(true);

            pointsText.text = "YOU MADE IT TO DUNGEON "+ score.ToString() +"and" + SceneManager.sceneCountInBuildSettings;
        }

        public void RestartButton()
        {
            //Destroy(this.gameObject);
            SceneManager.LoadScene("Scences/Home");
            Console.WriteLine(SceneManager.sceneCountInBuildSettings);
        }

        public void ExitButton()
        {
            //Destroy(this.gameObject);
            SceneManager.LoadScene("Scenes/SampleScene");
            Console.WriteLine(SceneManager.sceneCountInBuildSettings);

        }
    }
