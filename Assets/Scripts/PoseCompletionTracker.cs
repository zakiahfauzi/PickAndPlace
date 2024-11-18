using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Oculus.Interaction.Samples;

public class PoseCompletionTracker : MonoBehaviour
{
    [SerializeField]
    private PoseUseSample poseUseSampleScript;  // Reference to the PoseUseSample script

    [SerializeField]
    private TextMeshProUGUI instructionText;    // UI Text for instructions

    [SerializeField]
    private TextMeshProUGUI successMessage;     // UI Text to display success message

    [SerializeField]
    private GameObject[] handModels;           // Array of hand models

    private HashSet<int> completedPoses = new HashSet<int>(); // Track completed poses

    private int currentPoseIndex = -1; // Index of the currently displayed pose

    private void Start()
    {
        successMessage.gameObject.SetActive(false); // Hide success message initially
        instructionText.text = "Perform the poses shown to succeed.";

        // Subscribe to pose events
        for (int i = 0; i < poseUseSampleScript._poses.Length; i++)
        {
            int poseIndex = i; // Capture loop variable
            poseUseSampleScript._poses[poseIndex].WhenSelected += () => OnPoseCompleted(poseIndex);
        }

        ShowNextPose(); // Start with the first pose
    }

    private void ShowNextPose()
    {
        // Hide all hand models
        foreach (var handModel in handModels)
        {
            handModel.SetActive(false);
        }

        // Check if all poses are completed
        if (completedPoses.Count >= handModels.Length)
        {
            DisplaySuccess();
            return;
        }

        // Select a random pose not yet completed
        do
        {
            currentPoseIndex = Random.Range(0, handModels.Length);
        } while (completedPoses.Contains(currentPoseIndex));

        // Display the selected hand model
        handModels[currentPoseIndex].SetActive(true);

        // Update instruction text
        instructionText.text = $"Perform the pose: {poseUseSampleScript._poses[currentPoseIndex].name}";
    }

    private void OnPoseCompleted(int poseIndex)
    {
        if (poseIndex == currentPoseIndex && !completedPoses.Contains(poseIndex))
        {
            completedPoses.Add(poseIndex);
            ShowNextPose(); // Move to the next pose
        }
    }

    private void DisplaySuccess()
    {
        instructionText.text = ""; // Clear instruction text
        successMessage.gameObject.SetActive(true);
        successMessage.text = "Success! All poses completed!";
    }
}
