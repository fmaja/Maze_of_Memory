using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogi2 : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;
    public GameObject bot;

    public AudioClip strona;
    public AudioClip robot;
    public AudioClip powitanie;

    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool dialogueCompleted = false;
    private bool czyWidoczny = false;

    void Start()
    {
        dialoguePanel.SetActive(false);

        if (bot != null)
            bot.SetActive(false); // Bot ukryty na start
    }

    void Update()
    {
        if (isDialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ShowNextLine();
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                ShowPreviousLine();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dialogueCompleted)
        {
            if (bot != null)
            {
                bot.SetActive(true);
                if (!czyWidoczny)
                {
                    AudioManager.Instance.PlaySFX(robot);
                    czyWidoczny = true;
                    StartCoroutine(PlayDelayedSound());
                }

            }

            StartDialogue();
        }
    }

    private IEnumerator PlayDelayedSound()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySFX(powitanie);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isDialogueActive)
        {
            EndDialogue(false); // zakoñcz bez oznaczenia jako przeczytane
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        currentLineIndex = 0;
        dialogueText.text = dialogueLines[currentLineIndex];
    }

    void ShowNextLine()
    {
        AudioManager.Instance.PlaySFX(strona);

        if (currentLineIndex < dialogueLines.Length - 1)
        {
            currentLineIndex++;
            dialogueText.text = dialogueLines[currentLineIndex];
        }
        else
        {
            EndDialogue(true); // zakoñcz i oznacz jako przeczytane
        }
    }

    void ShowPreviousLine()
    {
        AudioManager.Instance.PlaySFX(strona);

        if (currentLineIndex > 0)
        {
            currentLineIndex--;
            dialogueText.text = dialogueLines[currentLineIndex];
        }
    }

    void EndDialogue(bool completed)
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);

        if (completed)
        {
            dialogueCompleted = true;

            FindObjectOfType<GameTimer>()?.StartTimer();

            if (bot != null)
                AudioManager.Instance.PlaySFX(robot);
            bot.SetActive(false); 
        }
    }
}
