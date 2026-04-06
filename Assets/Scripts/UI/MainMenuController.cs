using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainButtons;
    public GameObject gameModePanel;
    public GameObject howToPlayPanel;
    public GameObject playerSelectionPanel;

    public void ShowGameModes()
    {
        mainButtons.SetActive(false);
        gameModePanel.SetActive(true);
    }

    public void ShowHowToPlay()
    {
        mainButtons.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void BackToMain()
    {
        gameModePanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        playerSelectionPanel.SetActive(false);
        mainButtons.SetActive(true);
    }


    public void BackToGameMode()
    {
        gameModePanel.SetActive(true);
        howToPlayPanel.SetActive(false);
        playerSelectionPanel.SetActive(false);
        mainButtons.SetActive(false);
    }
    public void StartOnePlayer()
    {
        GameSettings.isSinglePlayer = true;
        SceneManager.LoadScene("Level_Easy"); // or difficulty selector later
    }

    public void StartTwoPlayer()
    {
        GameSettings.isSinglePlayer = false;
        SceneManager.LoadScene("Level_Easy");   // change this when you handle the scences يا ملكة
    }

    public void ShowPlayerSelection()
    {
        gameModePanel.SetActive(false);
        playerSelectionPanel.SetActive(true);
    }

    public void SelectPlayer1()
    {
        GameSettings.isSinglePlayer = true;
        GameSettings.humanPlaysAsPlayer1 = true;  // and this
        SceneManager.LoadScene("Level_Easy"); 
    }

    public void SelectPlayer2()
    {
        GameSettings.isSinglePlayer = true;
        GameSettings.humanPlaysAsPlayer1 = false;
        SceneManager.LoadScene("Level_Easy");   //this one too
    }


}