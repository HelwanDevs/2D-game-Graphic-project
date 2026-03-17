using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public ProjectileSpawner player1; // Hager
    public ProjectileSpawner player2; // Mariam

    private int currPlayer = 1;
    private bool switching = false;

    private void Start()
    {
        SetTurn(currPlayer);
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
        yield return new WaitForSeconds(2f); // wait for the projectile to land and animations to finish

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