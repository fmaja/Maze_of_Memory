using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Zagadka1 : MonoBehaviour
{
    public AudioClip efekt;

    private float czasOstatniegoKlikniecia;
    public float przerwa = 0.3f;
    public GameObject[] objekt1;
    public GameObject[] objekt2;
    public GameObject[] objekt3;

    private int licznik1 = 1;
    private int licznik2 = 1;
    private int licznik3 = 1;

    public UnityEvent wygranie;

    
    public void Wygrana()
    {
        if(objekt1[1].activeInHierarchy && objekt2[2].activeInHierarchy && objekt3[1].activeInHierarchy)
        {
            AudioManager.Instance.PlaySFX(efekt);
            wygranie.Invoke();
        }

    }

   
    public void konsola1()
    {
        if (Time.time - czasOstatniegoKlikniecia < przerwa)
            return;

        czasOstatniegoKlikniecia = Time.time;


        switch (licznik1)
        {
            case 0:
                objekt1[0].SetActive(true);
                objekt1[1].SetActive(false);
                objekt1[2].SetActive(false);
                break;

            case 1:
                objekt1[0].SetActive(false);
                objekt1[1].SetActive(true);
                objekt1[2].SetActive(false);
                break;

            case 2:
                objekt1[0].SetActive(false);
                objekt1[1].SetActive(false);
                objekt1[2].SetActive(true);
                break;
        }

        licznik1++;
        if (licznik1 > 2)
            licznik1 = 0;
    }

    public void konsola2()
    {
        if (Time.time - czasOstatniegoKlikniecia < przerwa)
            return;

        czasOstatniegoKlikniecia = Time.time;


        switch (licznik2)
        {
            case 0:
                objekt2[0].SetActive(true);
                objekt2[1].SetActive(false);
                objekt2[2].SetActive(false);
                break;

            case 1:
                objekt2[0].SetActive(false);
                objekt2[1].SetActive(true);
                objekt2[2].SetActive(false);
                break;

            case 2:
                objekt2[0].SetActive(false);
                objekt2[1].SetActive(false);
                objekt2[2].SetActive(true);
                break;
        }

        licznik2++;
        if (licznik2 > 2)
            licznik2 = 0;
    }

    public void konsola3()
    {
        if (Time.time - czasOstatniegoKlikniecia < przerwa)
            return;

        czasOstatniegoKlikniecia = Time.time;


        switch (licznik3)
        {
            case 0:
                objekt3[0].SetActive(true);
                objekt3[1].SetActive(false);
                objekt3[2].SetActive(false);
                break;

            case 1:
                objekt3[0].SetActive(false);
                objekt3[1].SetActive(true);
                objekt3[2].SetActive(false);
                break;

            case 2:
                objekt3[0].SetActive(false);
                objekt3[1].SetActive(false);
                objekt3[2].SetActive(true);
                break;
        }

        licznik3++;
        if (licznik3 > 2)
            licznik3 = 0;
    }
}
