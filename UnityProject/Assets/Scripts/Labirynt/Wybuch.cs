using UnityEngine;

public class Wybuch : MonoBehaviour
{
    public AudioClip dzwiekWybuchu;

    private bool wybuchl = false;

    void ZrzucBombe(Vector3 pozycja)
    {
        if (wybuchl) return;
        wybuchl = true;

        if (dzwiekWybuchu != null)
            AudioManager.Instance.PlaySFX(dzwiekWybuchu);


        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (wybuchl) return;

        if (other.CompareTag("Bomba"))
        {
            Destroy(other.gameObject);
            ZrzucBombe(transform.position);
        }
    }
}
