using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class TurnManager : NetworkBehaviour
{
    public static TurnManager Instance { get; private set; }

    public ProjectileSpawner player1; // Hager
    public ProjectileSpawner player2; // Mariam

    public NetworkVariable<int> currPlayer = new NetworkVariable<int>(1);
    private bool switching = false;

    void Start()
    {
        if (!GameSettings.isNetworkMultiplayer)
        {
            int startPlayer = GameSettings.humanPlaysAsPlayer1 ? 1 : 2;

            // in 2P mode always start with player 1
            if (!GameSettings.isSinglePlayer)
                startPlayer = 1;

            SetStartingTurn(startPlayer);
        }
        if (GameSettings.isNetworkMultiplayer)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                SetStartingTurn(1);
            }


        }
    }


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetStartingTurn(int playerNumber)
    {
        if (GameSettings.isNetworkMultiplayer && !NetworkManager.Singleton.IsServer)
        {
            Debug.LogWarning("Only the server can set the starting turn in a networked game.");
            return;
        }
        currPlayer.Value = playerNumber;
        SetTurn(playerNumber);
    }


    public void NextTurn()
    {
        if (!switching)
        {
            StartCoroutine(TurnSwitching());
        }
    }

    IEnumerator TurnSwitching()
    {
        switching = true;
        yield return new WaitForSeconds(2.0f); // wait for the projectile to land and animations to finish


        if (GameSettings.isNetworkMultiplayer && !NetworkManager.Singleton.IsServer)
        {
            Debug.LogWarning("Only the server can switch turns in a networked game.");
            switching = false;
            yield break;
        }
        currPlayer.Value = (currPlayer.Value == 1) ? 2 : 1;
        SetTurn(currPlayer.Value);

        switching = false;
    }

    void SetTurn(int player)
    {
        if (player == 1)
        {
            Debug.Log("Player 1's turn");
            player1.canShoot = true;
            player2.canShoot = false;
        }
        else
        {
            Debug.Log("Player 2's turn");
            player1.canShoot = false;
            player2.canShoot = true;
        }
    }

    void OnEnable()
    {
        currPlayer.OnValueChanged += OnTurnChanged;
    }

    void OnDisable()
    {
        currPlayer.OnValueChanged -= OnTurnChanged;
    }

    void OnTurnChanged(int oldPlayer, int newPlayer)
    {
        SetTurn(newPlayer);
    }


}