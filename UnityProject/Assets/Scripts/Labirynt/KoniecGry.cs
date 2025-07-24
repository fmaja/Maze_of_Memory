using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class KoniecGry : MonoBehaviour
{
    public GameObject blackPanel;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public AudioClip hospitalSound;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        triggered = true;

        StartCoroutine(SekwencjaKoncowa());
    }

    IEnumerator SekwencjaKoncowa()
    {
        Time.timeScale = 0f;

        AudioManager.Instance.PlayMusic(hospitalSound);

        
        AudioSource[] audioManagerSources = AudioManager.Instance.GetComponentsInChildren<AudioSource>();

        
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource src in allAudioSources)
        {
            bool isInAudioManager = false;

            foreach (AudioSource managerSrc in audioManagerSources)
            {
                if (src == managerSrc)
                {
                    isInAudioManager = true;
                    break;
                }
            }

            if (!isInAudioManager)
            {
                src.Stop(); 
            }
        }



        blackPanel.SetActive(true);

        yield return StartCoroutine(PoczekajRealTime(2f));
        text1.gameObject.SetActive(true);
        text1.text = "Jestem w szpitalu? Czy moje wspomnienia s¹ bezpieczne?";

        yield return StartCoroutine(PoczekajRealTime(3f));
        text2.gameObject.SetActive(true);
        text2.text = "Tak, panie majorze. Pana wspomnienia nigdy nie by³y zagro¿one. Swoim wielkim umys³em przys³u¿y³ siê pan nauce jak ma³o kto. Dziêkujemy.";

        yield return StartCoroutine(PoczekajRealTime(6f));

        PowrotDoMenu();
        
    }

    
    IEnumerator PoczekajRealTime(float seconds)
    {
        float t = 0f;
        while (t < seconds)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public void PowrotDoMenu()
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
}

