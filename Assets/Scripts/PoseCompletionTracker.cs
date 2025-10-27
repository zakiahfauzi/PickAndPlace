using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Oculus.Interaction.Samples;

public class PoseCompletionTracker : MonoBehaviour
{
    [SerializeField]
    private PoseUseSample poseUseSampleScript;

    [SerializeField]
    private TextMeshProUGUI instructionText;

    [SerializeField]
    private TextMeshProUGUI successMessage;

    [SerializeField]
    private GameObject[] handModels;

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
        {
            handModel.SetActive(false);
        }

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

        // Move to Level 3 after 4 seconds
        Invoke(nameof(LoadNextLevel), 4f);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene("Level04");
    }

    private void SaveLevelData()
    {
        float timeTaken = Time.time - startTime;
        string filePath = Path.Combine(Application.dataPath, "level_data.txt");

        string levelData = "Level: 2\n";
        levelData += "Time Taken: " + timeTaken + " seconds\n";
        levelData += "Wrong Poses: " + wrongPoses + "\n";

        File.AppendAllText(filePath, levelData + "\n");
        Debug.Log("Level data saved to: " + filePath);
    }
}
