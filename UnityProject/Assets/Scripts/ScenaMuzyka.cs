using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenaMuzykac : MonoBehaviour
{
    public AudioClip musicForScene;

    void Start()
    {
        AudioManager.Instance.PlayMusic(musicForScene);
    }
}