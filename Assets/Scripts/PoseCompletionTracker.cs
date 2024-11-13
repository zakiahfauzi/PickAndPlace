using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Oculus.Interaction.Samples;  

public class PoseCompletionTracker : MonoBehaviour
{
    [SerializeField]
    private PoseUseSample poseUseSampleScript;  // Reference to the PoseUseSample script
    [SerializeField]
    private TextMeshProUGUI successMessage;     // UI Text to display success message

    private HashSet<int> detectedPoses = new HashSet<int>();  // Track unique poses detected
    private int totalPoses = 6;  // Total number of poses to detect

    private void Start()
    {
        // Ensure the success message is hidden at the start
        successMessage.text = "Move your hands to perform all 6 poses.";
        successMessage.gameObject.SetActive(true);

        // Subscribe to pose events
        for (int i = 0; i < poseUseSampleScript._poses.Length; i++)
        {
            int poseIndex = i;  // Local copy of index for lambda capture
            poseUseSampleScript._poses[poseIndex].WhenSelected += () => OnPoseDetected(poseIndex);
        }
    }

    private void OnPoseDetected(int poseIndex)
    {
        // Add the detected pose index to the HashSet (no duplicates allowed)
        detectedPoses.Add(poseIndex);

        // Check if all poses have been detected
        if (detectedPoses.Count == totalPoses)
        {
            ShowSuccessMessage();
        }
    }

    private void ShowSuccessMessage()
    {
        successMessage.text = "Success! All 6 poses detected!";
        successMessage.gameObject.SetActive(true);
    }
}
