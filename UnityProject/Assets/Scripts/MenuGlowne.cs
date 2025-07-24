using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGlowne : MonoBehaviour
{ 
    public string targetScene;
    public GameObject pomoc;

    public void Pomoc()
    {
        pomoc.SetActive(true);
    }

    public void PrzejscieDoMapy()
    {
        SceneManager.LoadScene(targetScene);
    }

    public void Wyjscie()
    {
        Application.Quit();
    }
}