using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class ReturnMainMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        if (GameSettings.isNetworkMultiplayer)
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.Shutdown();
            }
        }
        SceneManager.LoadScene("Main_Menu");
    }
}