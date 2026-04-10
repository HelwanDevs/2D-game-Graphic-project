using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public ProjectileSpawner player1; // Hager
    public ProjectileSpawner player2; // Mariam

    public  int currPlayer = 1;
    private bool switching = false;

    void Start()
    {
        int startPlayer = GameSettings.humanPlaysAsPlayer1 ? 1 : 2;

        // in 2P mode always start with player 1
        if (!GameSettings.isSinglePlayer)
            startPlayer = 1;

        SetStartingTurn(startPlayer);
    }


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetStartingTurn(int playerNumber)
    {
        currPlayer = playerNumber;
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

        currPlayer = (currPlayer == 1) ? 2 : 1;
        SetTurn(currPlayer);

        switching = false; 
    }

    void SetTurn(int player)
    {
        if (player == 1)
        {
            player1.canShoot = true;
            player2.canShoot = false;
        }
        else
        {
            player1.canShoot = false;
            player2.canShoot = true;
        }
    }
}