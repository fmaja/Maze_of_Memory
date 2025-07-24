using UnityEngine;
using UnityEngine.SceneManagement;

public class UIKontroler : MonoBehaviour
{
    public GameObject splashPanel;
    public GameObject mainMenuPanel;
    public string targetScene;

    public static UIKontroler Instance;

    private static bool splashShown = false;

    void Start()
    {
        if (!splashShown)
        {
            splashShown = true;
            ShowSplash();
        }
        else
        {
            ShowMainMenu();
        }
    }

    void Awake()
    {
        Instance = this;
    }

    public void ShowSplash()
    {
        splashPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        Invoke(nameof(ShowMainMenu), 4f);
    }

    public void ShowMainMenu()
    {
        splashPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        SceneManager.LoadScene(targetScene);
    }

    public void ReturnToMenu()
    {
        mainMenuPanel.SetActive(true);
    }
}

