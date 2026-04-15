using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class WinUI : MonoBehaviour
{
    public TMP_Text winnerText;
    public GameObject nextLevelButton;

    void Start()
    {
        if (GameSettings.winnerName != null)
        {
            winnerText.text = $"{GameSettings.winnerName} Wins!";
        }
        else if (winnerText == null)
        {

        }

        // show next level only if the player won and next level exists
        nextLevelButton.SetActive(
            !GameSettings.isNetworkMultiplayer &&
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