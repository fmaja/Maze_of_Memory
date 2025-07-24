using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitMenuController : MonoBehaviour
{
    public GameObject exitMenuPanel;
    public GameObject helpPanel;
    public GameObject settingsPanel;

    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                ShowExitMenu();
            else
                HideExitMenu();
        }
    }

    public void ShowExitMenu()
    {
        exitMenuPanel.SetActive(true);
        isPaused = true;

        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideExitMenu()
    {
        exitMenuPanel.SetActive(false);
        isPaused = false;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowHelpPanel()
    {
        helpPanel.SetActive(true);
    }

    public void ShowSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }
    public void OnExitConfirmed()
    {
        Time.timeScale = 1f; // przywróæ czas na wypadek
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

       
        var allObjects = FindObjectsOfType<GameObject>(true); 
        var audioManager = GameObject.Find("AudioManager");

        foreach (var obj in allObjects)
        {
            if (obj.scene.name == "DontDestroyOnLoad")
            {
                if (obj == audioManager || obj.transform.IsChildOf(audioManager.transform))
                    continue;

                Destroy(obj);
            }
        }

        SceneManager.LoadScene("MainMenu");
    }
}
