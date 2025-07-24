using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Zycia : MonoBehaviour
{
    public static Zycia Instance;

    public AudioClip dzwiek;

    public Image[] hearts;

    public GameObject gameOverPanel; 
    public Button menuButton;      

    private int currentLives;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        currentLives = hearts.Length;
        UpdateHeartsUI();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false); 

        if (menuButton != null)
            menuButton.onClick.AddListener(BackToMenu);
    }

    public void LoseLife()
    {
        if (currentLives <= 0) return;

        AudioManager.Instance.PlaySFX(dzwiek);

        currentLives--;
        UpdateHeartsUI();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    public void GainLife()
    {
        if (currentLives >= hearts.Length) return;

        currentLives++;
        UpdateHeartsUI();
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentLives;
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f; 
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoseLife();
        }
    }
}
