using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // For the Timer Display
    public Button startButton; // Start Button
    public Button stopButton;  // Stop Button

    private float elapsedTime;
    private bool isTiming;

    void Start()
    {
        isTiming = false;
        elapsedTime = 0;
        timerText.text = "00:00"; // Initial Timer Display

        startButton.onClick.AddListener(StartTimer);
        stopButton.onClick.AddListener(StopTimer);
    }

    void Update()
    {
        if (isTiming)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void StartTimer()
    {
        isTiming = true;
        elapsedTime = 0;
    }

    private void StopTimer()
    {
        isTiming = false;
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
