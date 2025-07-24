using UnityEngine;

public class ConsoleTrigger : MonoBehaviour
{
    public GameObject openPasswordButton;
    public GameObject UI;
    public GameObject objectToUnlock;
    public bool firstActive = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && objectToUnlock.activeInHierarchy == firstActive)
        {
            openPasswordButton.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openPasswordButton.SetActive(false);
            UI.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
