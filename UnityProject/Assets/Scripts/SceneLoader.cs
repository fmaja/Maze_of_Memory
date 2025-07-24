using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    public string targetScene;

    private string currentScene;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // nie niszcz przy zmianie scen
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Na start wczytaj np. pierwsz¹ scenê gry:
        LoadLevel(targetScene);
    }

    public void LoadLevel(string sceneName)
    {
        if (!string.IsNullOrEmpty(currentScene))
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }

        SceneManager.LoadScene(sceneName);
        currentScene = sceneName;
    }
}
