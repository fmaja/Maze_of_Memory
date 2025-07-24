using UnityEngine;
using UnityEngine.SceneManagement;

public class poczatek : MonoBehaviour
{
    public string targetScene;

    void Start()
    {
        Invoke(nameof(GoMainMenu), 4f);
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene(targetScene);
    }
}