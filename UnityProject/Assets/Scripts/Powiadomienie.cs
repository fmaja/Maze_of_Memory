using UnityEngine;
using UnityEngine.UI;

public class Powiadomienie : MonoBehaviour
{
    public GameObject infoPanel;           
    public float czasPokazaniaSekundy = 4f; 

    private bool pokazano = false;

    private void OnTriggerEnter(Collider other)
    {
        if (pokazano) return;

        if (other.CompareTag("Player")) 
        {
            pokazano = true;
            StartCoroutine(PokazPanelPrzezChwile());
        }
    }

    private System.Collections.IEnumerator PokazPanelPrzezChwile()
    {
        infoPanel.SetActive(true);
        yield return new WaitForSeconds(czasPokazaniaSekundy);
        infoPanel.SetActive(false);
    }
}
