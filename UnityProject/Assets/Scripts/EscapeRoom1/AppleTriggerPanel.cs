using System.Collections.Generic;
using UnityEngine;

public class BateriaTriggerPanel : MonoBehaviour
{
    public GameObject panel1; 
    public GameObject panel2;
    public GameObject panel3;
    public GameObject obiekt1;
    public GameObject obiekt2;

    public AudioClip efekt;

    private HashSet<GameObject> baterieWTriggerze = new HashSet<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bateria"))
        {
            baterieWTriggerze.Add(other.gameObject);
            Destroy(other.gameObject);
            CheckBaterie();
            AudioManager.Instance.PlaySFX(efekt);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bateria"))
        {
            baterieWTriggerze.Remove(other.gameObject);
        }
    }

    void CheckBaterie()
    {
        if (baterieWTriggerze.Count >= 3)
        {
            // Dezaktywuj panel1
            if (panel1 != null) panel1.SetActive(false);

            // Aktywuj panel2
            if (panel2 != null) panel2.SetActive(true);
            if (panel3 != null) panel3.SetActive(true);

            // Aktywuj obiekty
            if (obiekt1 != null) obiekt1.SetActive(true);
            if (obiekt2 != null) obiekt2.SetActive(true);

            baterieWTriggerze.Clear();
        }
    }
}
