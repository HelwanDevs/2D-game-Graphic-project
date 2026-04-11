using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    public TMP_Text winnerText;
    public GameObject nextLevelButton;

    void Start()
    {
        winnerText.text = $"{GameSettings.winnerName} Wins!";

        // show next level only if the player won and next level exists
        nextLevelButton.SetActive(
            GameSettings.isSinglePlayer &&
            !GameSettings.humanLost &&
            GetNextScene() != null
        );
    }

    public void GoToNextLevel()
    {
        string next = GetNextScene();
        if (next != null)
            SceneManager.LoadScene(next);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    string GetNextScene()
    {
        switch (GameSettings.currentScene)
        {
            case "Level_Easy": return "Level_Medium";
            case "Level_Medium": return "Level_Hard";
            default: return null; // Hard or 2P
        }
    }
}