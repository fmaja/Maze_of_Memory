using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class FotoCollecter : MonoBehaviour
{
    public GameObject panelDialog;
    public GameObject panelWygrana;

    public AudioClip efekt;

    private HashSet<GameObject> zdjecieWTriggerze = new HashSet<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Foto"))
        {
            zdjecieWTriggerze.Add(other.gameObject);
            Destroy(other.gameObject);
            CheckBaterie();
            AudioManager.Instance.PlaySFX(efekt);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bateria"))
        {
            zdjecieWTriggerze.Remove(other.gameObject);
        }
    }

    void CheckBaterie()
    {
        if (zdjecieWTriggerze.Count >= 5)
        {
        
            if (panelDialog != null) panelDialog.SetActive(false);

            // Aktywuj panel2
            if (panelWygrana != null) panelWygrana.SetActive(true);

            StartCoroutine(HandleWinSequence());



        }
    }

    IEnumerator HandleWinSequence()
    {
        

        yield return new WaitForSeconds(3f);

   
        SceneManager.LoadScene("Puzzle");
    }
}
