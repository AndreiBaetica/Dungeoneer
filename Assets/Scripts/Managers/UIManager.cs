using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Singleton

    public static UIManager instance;
    
    void Awake()   
    {
        instance = this;
    }
        
    #endregion


    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    public static Text[] pointsText;
    public Canvas[] gameUICanvas;
    private static Canvas gameOverScreen;
    private static Canvas gameUI;
    private static Canvas pauseMenu;
    public static int FinalRoomScore;
    private static bool acceptingInput;
    void Start()
    {
        gameUICanvas = FindObjectsOfType<Canvas>();
        for (int i = 0; i < gameUICanvas.Length; i++)
        {
            if (gameUICanvas[i].name == "GameOverMenu")
            {
                gameOverScreen = gameUICanvas[i];
            }
            if (gameUICanvas[i].name == "GameMenu")
            {
                gameUI = gameUICanvas[i];
            }
            if (gameUICanvas[i].name == "PauseMenu")
            {
                pauseMenu = gameUICanvas[i];
            }

        }

        ResumeGame();
    }
    
    
    
    public static void EndGame()
    {
        PauseGameTime();
        acceptingInput = false;
        gameOverScreen.enabled = true;
        gameUI.enabled = false;
        pauseMenu.enabled = false;

       int foundTextIndex=0;
       pointsText = FindObjectsOfType<Text>();
       for (int i = 0; i < pointsText.Length; i++)
       {
           if (pointsText[i].name == "SCORE")
           {
               pointsText[i].text = "YOU MADE IT TO DUNGEON "+ FinalRoomScore.ToString();
               foundTextIndex = i;
           }
       }
      
       Debug.Log("Found Object: " + pointsText[foundTextIndex].text);
    }
    
    public static bool PauseGame()
    {
        if (IsFrozen())
        {
            bool isFree;
            isFree = ResumeGame();
            return isFree;
        }
        PauseGameTime();
        gameOverScreen.enabled = false;
        gameUI.enabled = false;
        pauseMenu.enabled = true;
        return false;
    }
    
    public static bool ResumeGame()
    {
        acceptingInput = true;
        ResumeGameTime();
        gameOverScreen.enabled = false;
        gameUI.enabled = true;
        pauseMenu.enabled = false;
        return true;
    }
    
    private static void PauseGameTime()
    {
        Time.timeScale = 0;
    }

    private static void ResumeGameTime()
    {
        Time.timeScale = 1;
    }

    public static bool IsFrozen()
    {
        return Time.timeScale.Equals(0);
    }

    private void Update()
    {
        if (acceptingInput)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }

        if (SceneManager.GetActiveScene().name == "TestDungeon")
        {
            TextMeshPro[] TMP = gameUI.GetComponents<TextMeshPro>();
            Debug.Log(TMP.Length);
        }

    }

    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1)
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }
        else
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
            clickable.MyIcon.color = Color.white;
        }
        if (clickable.MyCount == 0)
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0); //Reset slot icon to transparent if it is empty
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }
}


