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
            pointsText.text = "YOU MADE IT TO DUNGEON "+ score.ToString() ;
        }

        public void RestartButton()
        {
            SceneManager.LoadScene("Scenes/SampleScene");
        }

        public void ExitButton()
        {
            SceneManager.LoadScene("Scenes/Home");
        }
    }
