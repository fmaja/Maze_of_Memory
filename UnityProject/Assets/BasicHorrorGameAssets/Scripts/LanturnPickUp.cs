using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanturnPickUp : MonoBehaviour
{
    private GameObject OB;
    private GameObject lanturn;

    void Start()
    {
        OB = this.gameObject;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            
            Transform[] allChildren = player.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if (child.name == "Lanturn")
                {
                    lanturn = child.gameObject;
                    lanturn.SetActive(false);
                    break;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (lanturn != null)
                lanturn.SetActive(true);

            Destroy(OB);
        }
    }
}
