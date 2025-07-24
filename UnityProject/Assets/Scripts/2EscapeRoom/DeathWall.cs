using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathWall : MonoBehaviour
{
    public GameObject scianaRuchoma;
    public Transform punktDocelowy;
    public float predkoscSciany = 2f;
    public float czasDoStartu = 3f;
    public float czasNaZagadke = 10f;
    public GameObject panelZagadki;
    public GameObject panelKoncowy;
    public AudioClip muzyka;
    public AudioClip start;
    public AudioClip dobrze;

    private bool aktywnaZagadka = false;
    private bool scianaWruchu = false;
    private float czasZagadki = 0f;
    private Vector3 startowaPozycja;
    private bool czyRozwiazane = false;

    void Start()
    {
        panelZagadki.SetActive(false);
        startowaPozycja = scianaRuchoma.transform.position;
        AudioManager.Instance.PlayMusic(muzyka);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !aktywnaZagadka && czyRozwiazane == false)
        {
            aktywnaZagadka = true;
            Invoke("StartZagadki", czasDoStartu);
        }
    }

    void StartZagadki()
    {
        panelZagadki.SetActive(true);
        scianaWruchu = true;
        czasZagadki = 0f;
        AudioManager.Instance.PlayMusic(start);
    }

    void Update()
    {
        if (!scianaWruchu || !aktywnaZagadka) return;

        czasZagadki += Time.deltaTime;

      
        scianaRuchoma.transform.position = Vector3.MoveTowards(
            scianaRuchoma.transform.position,
            punktDocelowy.position,
            predkoscSciany * Time.deltaTime
        );

        if (Vector3.Distance(scianaRuchoma.transform.position, punktDocelowy.position) < 0.1f)
        {
            Przegrana();
        }

        
        if (Input.GetKeyDown(KeyCode.Alpha1)) Odpowiedz(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Odpowiedz(2); // Poprawna
        if (Input.GetKeyDown(KeyCode.Alpha3)) Odpowiedz(3);

     
        if (czasZagadki >= czasNaZagadke)
        {
            Przegrana();
        }
    }

    void Odpowiedz(int numer)
    {
        if (numer == 2) 
        {
            scianaWruchu = false;
            StartCoroutine(CofnijSciane());
            AudioManager.Instance.PlaySFX(dobrze);
            czyRozwiazane = true;
            AudioManager.Instance.PlayMusic(muzyka);
        }
        else
        {
            Przegrana();
        }
    }

    void Przegrana()
    {
        Zycia.Instance.LoseLife();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    System.Collections.IEnumerator CofnijSciane()
    {
        yield return new WaitForSeconds(1f);
        while (Vector3.Distance(scianaRuchoma.transform.position, startowaPozycja) > 0.1f)
        {
            scianaRuchoma.transform.position = Vector3.MoveTowards(
                scianaRuchoma.transform.position,
                startowaPozycja,
                predkoscSciany * Time.deltaTime
            );
            yield return null;
        }

        panelZagadki.SetActive(false);
        panelKoncowy.SetActive(true);
        aktywnaZagadka = false;
    }
}
