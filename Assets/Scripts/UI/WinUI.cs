using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    public static WinUI Instance { get; private set; }

    [Header("References")]
    public GameObject winPanel;
    public TMP_Text winnerText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowWinner(string winnerName)
    {
        winPanel.SetActive(true);
        winnerText.text = $"{winnerName} Wins!";
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}