using System.Collections;
using System.IO;
using UnityEngine;

public class FaceDataLogger : MonoBehaviour
{
    private OVRFaceExpressions faceExpressions;
    private StreamWriter writer;
    private string filePath;

    private void Start()
    {
        // Generate a unique filename using the current timestamp
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        filePath = Path.Combine(Application.persistentDataPath, $"FaceDataLog_{timestamp}.txt");

        // Debug log to show the file path
        Debug.Log("Face data will be saved to: " + filePath);

        // Open a new StreamWriter for the unique file
        writer = new StreamWriter(filePath, true);
        writer.WriteLine("Timestamp,Expression,Weight");
        writer.Flush();

        // Attach OVRFaceExpressions component if it's not already set
        faceExpressions = GetComponent<OVRFaceExpressions>();
    }

    private void Update()
    {
        if (faceExpressions != null && faceExpressions.ValidExpressions)
        {
            foreach (OVRFaceExpressions.FaceExpression expression in System.Enum.GetValues(typeof(OVRFaceExpressions.FaceExpression)))
            {
                if (faceExpressions.TryGetFaceExpressionWeight(expression, out float weight))
                {
                    string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    writer.WriteLine($"{timestamp},{expression},{weight}");
                    writer.Flush();
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        writer?.Close();
    }
}
