using System.Collections;
using UnityEngine;

public class Spawanie : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(PrzesunGraczaPoZaladowaniu());
    }

    IEnumerator PrzesunGraczaPoZaladowaniu()
    {
        yield return new WaitForEndOfFrame(); // Mo¿na daæ te¿ WaitForSeconds(0.1f)

        GameObject player = GameObject.FindWithTag("Player");
        GameObject spawn = GameObject.Find("PlayerSpawn");

        if (player != null && spawn != null)
        {
            player.transform.position = spawn.transform.position;
            player.transform.rotation = spawn.transform.rotation;
        }
        else
        {
            Debug.LogWarning("Nie znaleziono gracza lub punktu spawnu.");
        }
    }
}