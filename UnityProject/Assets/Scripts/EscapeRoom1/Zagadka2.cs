using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public string requiredTag = "Key";
    public AudioClip efekt;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            // Znikaj¹ drzwi
            Destroy(gameObject);

            // Znika klucz
            Destroy(other.gameObject);

            AudioManager.Instance.PlaySFX(efekt);
        }
    }
}