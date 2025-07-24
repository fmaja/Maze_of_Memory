using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XEntity.InventoryItemSystem;

public class PasswordLock : MonoBehaviour
{
    public string haslo1 = "start";
    public string haslo2 = "START";
    public bool makeActive = false;
    public GameObject passwordUI;
    public GameObject openPasswordButton; 
    public TMP_InputField inputField;
    public TMP_Text feedbackText;
    public GameObject objectToUnlock;
    public MonoBehaviour playerMovementScript;
    public AudioClip efekt;


    private bool unlocked = false;

   
    public void OpenPasswordUI()
    {
        passwordUI.SetActive(true);
        openPasswordButton.SetActive(false);

        FirstPersonController controller = playerMovementScript as FirstPersonController;
        if (controller != null)
        {
            controller.playerCanMove = false;
            controller.cameraCanMove = false;
        }
        

    }

    public void CheckPassword()
    {
        if (unlocked) return;

        if (inputField.text == haslo1 || inputField.text == haslo2)
        {
            objectToUnlock.SetActive(makeActive);
            AudioManager.Instance.PlaySFX(efekt);
            unlocked = true;
            ClosePasswordUI();
        }
        else
        {
            feedbackText.text = "Błąd. Spróbuj ponownie.";
        }
    }

    public void ClosePasswordUI()
    {


        passwordUI.SetActive(false);

        if (!unlocked)
            openPasswordButton.SetActive(true);

        FirstPersonController controller = playerMovementScript as FirstPersonController;
        if (controller != null)
        {
            controller.playerCanMove = true;
            controller.cameraCanMove = true;
           
        }
    }
}
