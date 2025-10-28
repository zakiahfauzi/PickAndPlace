using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Oculus.Interaction.Samples;

public class PoseCompletionTracker : MonoBehaviour
{
    [SerializeField] private PoseUseSample poseUseSampleScript;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private TextMeshProUGUI successMessage;
    [SerializeField] private GameObject[] handModels;

    private HashSet<int> completedPoses = new HashSet<int>();
    private int currentPoseIndex = -1;
    private int wrongPoses = 0;
    private float startTime;
    private bool levelCompleted = false;

    private void Start()
    {
        successMessage.gameObject.SetActive(false);
        instructionText.text = "Perform the poses shown to succeed.";

        startTime = Time.time;

        // Register pose event listeners
        for (int i = 0; i < poseUseSampleScript._poses.Length; i++)
        {
            int poseIndex = i;
            poseUseSampleScript._poses[poseIndex].WhenSelected += () => OnPoseCompleted(poseIndex);
        }

        ShowNextPose();
    }

    private void ShowNextPose()
    {
        foreach (var handModel in handModels)
            handModel.SetActive(false);

        if (completedPoses.Count >= handModels.Length)
        {
            DisplaySuccess();
            return;
        }

        do
        {
            currentPoseIndex = Random.Range(0, handModels.Length);
        } while (completedPoses.Contains(currentPoseIndex));

        handModels[currentPoseIndex].SetActive(true);
        instructionText.text = $"Perform the pose: {poseUseSampleScript._poses[currentPoseIndex].name}";
    }

    private void OnPoseCompleted(int poseIndex)
    {
        if (poseIndex == currentPoseIndex && !completedPoses.Contains(poseIndex))
        {
            completedPoses.Add(poseIndex);
            ShowNextPose();
        }
        else
        {
            wrongPoses++;
        }

        if (completedPoses.Count == handModels.Length && !levelCompleted)
        {
            levelCompleted = true;
            SaveLevelData();
        }
    }

    private void DisplaySuccess()
    {
        instructionText.text = "";
        successMessage.gameObject.SetActive(true);
        successMessage.text = "Success! All poses completed!\nProceeding to Level 3...";

        Invoke(nameof(LoadNextLevel), 4f);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene("Level04");
    }

    private void SaveLevelData()
    {
        float timeTaken = Time.time - startTime;

        // Create unique timestamp for filename
        string ts = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = $"Level02_{ts}.txt";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        string content = $"Level: 2\n";
        content += $"Time Taken: {timeTaken:F2} seconds ({timeTaken / 60f:F2} minutes)\n";
        content += $"Wrong Poses: {wrongPoses}\n";
        content += $"Timestamp: {ts}\n";

        try
        {
            File.WriteAllText(filePath, content);
            Debug.Log($"[PoseCompletionTracker] Level 2 data saved to: {filePath}");
        }
        catch
        {
            Debug.LogError("[PoseCompletionTracker] Failed to save level data.");
        }
    }
}
