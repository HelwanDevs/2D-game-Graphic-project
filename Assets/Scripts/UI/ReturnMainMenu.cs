using UnityEngine;

public class ReturnMainMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {


        UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Menu");
    }
}