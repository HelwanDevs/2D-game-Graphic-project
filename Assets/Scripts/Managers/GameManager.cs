using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
        if (scene.name == "MainMenu") return;

        player1 = GameObject.Find("Player 1 (Hager)");
        player2 = GameObject.Find("Player 2 (Mariam)");

        SetupGameMode();
    }

    void SetupGameMode()
    {
        Debug.Log($"[GameManager] isSinglePlayer: {GameSettings.isSinglePlayer}");
        Debug.Log($"[GameManager] humanPlaysAsPlayer1: {GameSettings.humanPlaysAsPlayer1}");

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

    public enum PlayerID { Player1, Player2 }

    //public void OnPlayerDied(PlayerID loser)
    //{
    //    PlayerID winner = loser == PlayerID.Player1
    //        ? PlayerID.Player2
    //        : PlayerID.Player1;

    //    Debug.Log($"{winner} wins!");
    //    Invoke(nameof(LoadMainMenu), 3f);
    //}

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnPlayerDied(PlayerID loser)
    {
        string winner = loser == PlayerID.Player1 ? "Player 2" : "Player 1";
        WinUI.Instance.ShowWinner(winner);
    }

    public Transform GetPlayer1Transform() => player1.transform;
    public Transform GetPlayer2Transform() => player2.transform;
}