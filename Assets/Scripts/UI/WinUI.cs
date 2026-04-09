using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    public TMP_Text winnerText;

    void Start()
    {
        winnerText.text = $"{GameSettings.winnerName} Wins!";
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}