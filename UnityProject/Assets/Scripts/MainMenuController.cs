using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("UIManager"); // Nazwa sceny docelowej
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Zamykanie gry");
    }
}