using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // For the Timer Display
    public Button startButton; // Start Button
    public Button stopButton;  // Stop Button
    public Button restartButton; // Restart Button

    private float elapsedTime;
    private bool isTiming;
    private string filePath;

    void Start()
    {
        isTiming = false;
        elapsedTime = 0;
        timerText.text = "00:00"; // Initial Timer Display
        filePath = Application.persistentDataPath + "/TotalTime.txt"; // Define file path

        startButton.onClick.AddListener(StartTimer);
        stopButton.onClick.AddListener(StopTimer);
        restartButton.onClick.AddListener(RestartTimer); // Add listener for restart button
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
        SaveTimeToFile();
    }

    private void RestartTimer()
    {
        // Reset timer and display
        isTiming = false;
        elapsedTime = 0;
        timerText.text = "00:00"; // Reset timer display
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void SaveTimeToFile()
    {
        string formattedTime = timerText.text;
        File.AppendAllText(filePath, "Total Time: " + formattedTime + "\n");
        Debug.Log("Total time saved to: " + filePath);
    }
}
