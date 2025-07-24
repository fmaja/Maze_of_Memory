using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;

public class GameTimer : MonoBehaviour
{
    public AudioClip efekt;
    public float timeLimit = 600f;
    private float currentTime;
    private bool timerRunning = false;

    public TextMeshProUGUI timerText;

    void Start()
    {
        currentTime = timeLimit;
        timerRunning = false;

        // Ukryj timer na start
        if (timerText != null)
            timerText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (timerRunning)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                timerRunning = false;
                UpdateTimerText();

                Zycia.Instance.LoseLife();
                RestartTimer();
                return;
            }

            UpdateTimerText();
        }
    }

    public void StartTimer()
    {
        AudioManager.Instance.PlaySFX(efekt);
        currentTime = timeLimit;
        timerRunning = true;

        if (timerText != null)
            timerText.gameObject.SetActive(true);

        UpdateTimerText();
        StartCoroutine(PlayDelayedSound());
    }

    private IEnumerator PlayDelayedSound()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySFX(efekt);
    }

    public void RestartTimer()
    {
        currentTime = timeLimit;
        timerRunning = true;

        if (timerText != null)
            timerText.gameObject.SetActive(true);

        UpdateTimerText();
    }

    public void StopTimerOnWin()
    {
        timerRunning = false;

        if (timerText != null)
            timerText.gameObject.SetActive(false);
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public float GetTimeLeft()
    {
        return currentTime;
    }
}
