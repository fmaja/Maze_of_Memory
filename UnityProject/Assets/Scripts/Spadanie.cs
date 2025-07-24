using UnityEngine;

public class Spadanie : MonoBehaviour
{
    public Transform spawnPoint;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Zycia.Instance != null)
            {
                Zycia.Instance.LoseLife();
            }

            if (spawnPoint != null)
            {
                CharacterController cc = other.GetComponent<CharacterController>();
                if (cc != null)
                {
                    cc.enabled = false;
                    other.transform.position = spawnPoint.position;
                    cc.enabled = true;
                }
                else
                {
                    other.transform.position = spawnPoint.position;
                }
            }
        }
    }
}
