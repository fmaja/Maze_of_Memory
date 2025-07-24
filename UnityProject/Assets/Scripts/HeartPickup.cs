using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public int lifeAmount = 1;
    public AudioClip dzwiek;

    void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true; // na wszelki wypadek wymuœ trigger
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            Zycia.Instance.GainLife();

            AudioManager.Instance.PlaySFX(dzwiek);
            Destroy(gameObject);
        }
    }
}
