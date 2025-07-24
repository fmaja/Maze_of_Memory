using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Konsola : MonoBehaviour
{
    public AudioClip efekt;

    public GameObject ekran;

    RozszerzonaWersja przycisk;

    public UnityEvent wcisniecie;

    private void Awake()
    {
        przycisk = new RozszerzonaWersja();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LeanTween.scale(ekran, Vector3.one, 3).setEase(LeanTweenType.easeOutExpo);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LeanTween.scale(ekran, Vector3.zero, 3).setEase(LeanTweenType.easeInExpo);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
             if(przycisk.Player.KlawiszE.WasPerformedThisFrame())
            {
                AudioManager.Instance.PlaySFX(efekt);
                wcisniecie.Invoke();

            }
        }

    }

    public void OnEnable()
    {
        przycisk.Player.Enable();
    }

    public void OnDisable()
    {
        przycisk.Player.Disable();
    }
}
