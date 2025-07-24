using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KontrolaGracza : MonoBehaviour
{
    private static KontrolaGracza instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}
