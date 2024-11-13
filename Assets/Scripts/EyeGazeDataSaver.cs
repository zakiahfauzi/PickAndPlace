using System.Collections;
using System.IO;
using UnityEngine;
using Oculus.VR;
using System;

public class EyeGazeDataSaver : MonoBehaviour
{
    // The file path where the data will be saved
    private string filePath;

    // You can specify the name of the file
    private string fileName = "EyeGazeData.txt";

    // Eye tracking mode (set to World Space, Tracking Space, or Head Space)
    public OVREyeGaze.EyeTrackingMode trackingMode = OVREyeGaze.EyeTrackingMode.TrackingSpace;

    // Interval for saving data (in seconds)
    public float saveInterval = 1f;

    // Track the last time the data was saved
    private float lastSaveTime;

    private void Start()
    {
        // Set up file path
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        // Debug: Show where the file will be saved
        Debug.Log($"Eye gaze data will be saved to: {filePath}");

        // Check if file already exists, if not create it
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Timestamp, Eye Left X, Eye Left Y, Eye Left Z, Eye Left Gaze Direction X, Eye Left Gaze Direction Y, Eye Left Gaze Direction Z, Eye Right X, Eye Right Y, Eye Right Z, Eye Right Gaze Direction X, Eye Right Gaze Direction Y, Eye Right Gaze Direction Z, Confidence Left, Confidence Right\n");
        }

        // Initialize the time for saving data
        lastSaveTime = Time.time;
    }

    private void Update()
    {
        // If the interval has passed, save data
        if (Time.time - lastSaveTime >= saveInterval)
        {
            SaveEyeGazeData();
            lastSaveTime = Time.time;
        }
    }

    // Function to capture and save eye gaze data
    private void SaveEyeGazeData()
    {
        // Check if eye tracking is enabled
        if (!OVRPlugin.eyeTrackingEnabled)
        {
            Debug.LogWarning("Eye tracking is not enabled.");
            return;
        }

        // Get the eye gaze data for both eyes
        OVRPlugin.EyeGazesState eyeGazesState = new OVRPlugin.EyeGazesState();
        if (OVRPlugin.GetEyeGazesState(OVRPlugin.Step.Render, -1, ref eyeGazesState))
        {
            var leftEyeGaze = eyeGazesState.EyeGazes[0]; // Left eye data
            var rightEyeGaze = eyeGazesState.EyeGazes[1]; // Right eye data

            if (leftEyeGaze.IsValid && rightEyeGaze.IsValid)
            {
                // Extract gaze data for both eyes
                OVRPlugin.Posef leftPose = leftEyeGaze.Pose;
                OVRPlugin.Posef rightPose = rightEyeGaze.Pose;

                // Access position data
                Vector3 leftGazePosition = new Vector3(leftPose.Position.x, leftPose.Position.y, leftPose.Position.z);
                Vector3 rightGazePosition = new Vector3(rightPose.Position.x, rightPose.Position.y, rightPose.Position.z);

                // Access gaze direction (forward direction the eye is looking)
                Vector3 leftGazeDirection = new Vector3(leftPose.Orientation.x, leftPose.Orientation.y, leftPose.Orientation.z);
                Vector3 rightGazeDirection = new Vector3(rightPose.Orientation.x, rightPose.Orientation.y, rightPose.Orientation.z);

                // Access confidence data
                float confidenceLeft = leftEyeGaze.Confidence;
                float confidenceRight = rightEyeGaze.Confidence;

                // Format the data into a string
                string data = $"{Time.time}, {leftGazePosition.x}, {leftGazePosition.y}, {leftGazePosition.z}, " +
                              $"{leftGazeDirection.x}, {leftGazeDirection.y}, {leftGazeDirection.z}, " +
                              $"{rightGazePosition.x}, {rightGazePosition.y}, {rightGazePosition.z}, " +
                              $"{rightGazeDirection.x}, {rightGazeDirection.y}, {rightGazeDirection.z}, " +
                              $"{confidenceLeft}, {confidenceRight}";

                // Save the data to the file
                File.AppendAllText(filePath, data + "\n");

                // Debug log to verify the data is being captured
                Debug.Log($"Saved Eye Gaze Data: {data}");
            }
        }
        else
        {
            Debug.LogWarning("Failed to get eye gaze state.");
        }
    }
}
