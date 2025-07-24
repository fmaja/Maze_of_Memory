using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TriggerDoLabiryntu : MonoBehaviour
{
    public GameObject panelMatki;
    public TMP_Text tekstMatki;
    public string scenaDocelowa = "Labirynt";
    public float czasDoPrzejscia = 4f;

    private bool aktywowany = false;

    void Start()
    {
        panelMatki.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !aktywowany)
        {
            aktywowany = true;
            StartCoroutine(SekwencjaPrzejscia());
        }
    }

    System.Collections.IEnumerator SekwencjaPrzejscia()
    {
        panelMatki.SetActive(true);
        tekstMatki.text = "Synku, uciekaj! Oni chc¹ ci zrobiæ krzywdê!";

        yield return new WaitForSeconds(czasDoPrzejscia);

        SceneManager.LoadScene(scenaDocelowa);
    }
}
