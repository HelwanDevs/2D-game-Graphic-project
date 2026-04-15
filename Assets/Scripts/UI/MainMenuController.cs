using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public TMP_InputField ipAdd;
    public Unity.Netcode.Transports.UTP.UnityTransport transport;

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
        GameSettings.isNetworkMultiplayer = false;

        SceneManager.LoadScene("Level_Easy"); // or difficulty selector later
    }

    public void StartTwoPlayer()
    {
        GameSettings.isSinglePlayer = false;
        GameSettings.isNetworkMultiplayer = false;

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
        GameSettings.humanPlaysAsPlayer1 = true;
        GameSettings.isNetworkMultiplayer = false;

        SceneManager.LoadScene("Level_Easy");
    }

    public void SelectPlayer2()
    {
        GameSettings.isSinglePlayer = true;
        GameSettings.humanPlaysAsPlayer1 = false;
        GameSettings.isNetworkMultiplayer = false;

        SceneManager.LoadScene("Level_Easy");
    }

    public void Start2Player()
    {
        GameSettings.isSinglePlayer = false;
        GameSettings.isNetworkMultiplayer = false;
        SceneManager.LoadScene("Level_Easy"); // 2P always starts at Easy
    }

    public void HostNetwork()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }
        GameSettings.isNetworkMultiplayer = true;
        GameSettings.isSinglePlayer = false;
        GameSettings.isHost = true;
        NetworkManager.Singleton.StartHost();

        NetworkManager.Singleton.SceneManager.LoadScene("Level_Easy", LoadSceneMode.Single);
    }

    public void JoinNetwork()
    {
        GameSettings.isNetworkMultiplayer = true;
        GameSettings.isSinglePlayer = false;

        transport = NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();
        string targetIP = string.IsNullOrEmpty(ipAdd.text) ? "127.0.0.1" : ipAdd.text;
        transport.SetConnectionData(targetIP, 7777);
        NetworkManager.Singleton.StartClient();
    }


}