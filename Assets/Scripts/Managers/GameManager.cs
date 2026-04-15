using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum PlayerID { Player1, Player2 }
    public static GameManager Instance { get; private set; }

    [Header("Computer")]
    [Range(0f, 1f)]
    public float computerAccuracy = 0.7f;

    private GameObject player1;
    private GameObject player2;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main_Menu" || scene.name == "Win_Scene") return;

        player1 = GameObject.Find("Player 1 (Hager)");
        player2 = GameObject.Find("Player 2 (Mariam)");

        // set difficulty based on scene
        switch (scene.name)
        {
            case "Level_Easy": computerAccuracy = 0.5f; break;
            case "Level_Medium": computerAccuracy = 0.8f; break;
            case "Level_Hard": computerAccuracy = 1.0f; break;
        }
        Debug.Log($"[GameManager] Scene: {scene.name}, computerAccuracy: {computerAccuracy}");

        SetupGameMode();
    }

    void SetupGameMode()
    {
        Debug.Log($"[GameManager] isSinglePlayer: {GameSettings.isSinglePlayer}");
        Debug.Log($"[GameManager] humanPlaysAsPlayer1: {GameSettings.humanPlaysAsPlayer1}");
        Debug.Log($"[GameManager] isNetworkMultiplayer: {GameSettings.isNetworkMultiplayer}");


        if (GameSettings.isNetworkMultiplayer)
        {
            StartCoroutine(SetupNetworkGame());

        }

        if (player1 == null || player2 == null)
        {
            Debug.LogError("Players not found!");
            return;
        }

        // 2 Players Mode
        if (!GameSettings.isSinglePlayer)
        {
            SetPlayerInput(player1, true);
            SetPlayerInput(player2, true);
            return;
        }

        // 1 Player Mode 
        GameObject human = GameSettings.humanPlaysAsPlayer1 ? player1 : player2;
        GameObject computer = GameSettings.humanPlaysAsPlayer1 ? player2 : player1;

        SetPlayerInput(human, true);
        SetPlayerInput(computer, false);

        if (!computer.TryGetComponent(out ComputerController controller))
            controller = computer.AddComponent<ComputerController>();

        controller.accuracy = computerAccuracy;
        controller.playerNumber = GameSettings.humanPlaysAsPlayer1 ? 2 : 1;
        controller.Init();

        // set starting turn to human after everything is set up
        int humanPlayer = GameSettings.humanPlaysAsPlayer1 ? 1 : 2;
        TurnManager.Instance.SetStartingTurn(humanPlayer);
    }

    void SetPlayerInput(GameObject player, bool enabled)
    {
        var input = player.GetComponent<PlayerInput>();
        if (input != null) input.enabled = enabled;
    }

    IEnumerator SetupNetworkGame()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer);

        var p1 = player1.GetComponent<NetworkObject>();
        var p2 = player2.GetComponent<NetworkObject>();

        yield return new WaitUntil(() => p1.IsSpawned && p2.IsSpawned);

        p1.ChangeOwnership(NetworkManager.Singleton.LocalClientId);

        yield return new WaitUntil(() => NetworkManager.Singleton.ConnectedClientsList.Count > 1);


        p2.ChangeOwnership(NetworkManager.Singleton.ConnectedClientsList[1].ClientId);
        Debug.Log($"Ownership Assigned: Host owns {player1.name}, Client {NetworkManager.Singleton.ConnectedClientsList[1].ClientId} owns {player2.name}");

    }




    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnPlayerDied(PlayerID loser)
    {
        GameSettings.currentScene = SceneManager.GetActiveScene().name;

        if (GameSettings.isSinglePlayer)
        {
            // check if the player loses
            bool humanIsPlayer1 = GameSettings.humanPlaysAsPlayer1;
            bool humanLost = (humanIsPlayer1 && loser == PlayerID.Player1) ||
                             (!humanIsPlayer1 && loser == PlayerID.Player2);

            GameSettings.humanLost = humanLost;
            GameSettings.winnerName = humanLost ? "Computer" : "You";
        }
        else
        {
            GameSettings.humanLost = false;
            GameSettings.winnerName = loser == PlayerID.Player1 ? "Player 2" : "Player 1";
        }

        SceneManager.LoadScene("Win_Scene");
    }

    public Transform GetPlayer1Transform() => player1.transform;
    public Transform GetPlayer2Transform() => player2.transform;


    void OnApplicationQuit()
    {
        if (NetworkManager.Singleton != null &&
            NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.Shutdown();
        }
    }
    void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }
    }
}