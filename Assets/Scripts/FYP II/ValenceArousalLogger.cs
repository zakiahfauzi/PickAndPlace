using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ValenceArousalLogger : MonoBehaviour
{
    [Header("Emotion Source (Automatically found if empty)")]
    [Tooltip("Component which implements IEmotionSource (e.g., ONNXEmotionPredictor).")]
    public MonoBehaviour emotionSource; // must implement IEmotionSource

    [Header("Logging Settings")]
    [Tooltip("Base filename (scene name and timestamp will be added automatically).")]
    public string baseFileName = "valence_arousal_log";

    [Tooltip("Minimum seconds between writes to disk (throttle). Set to 0 to write every frame.")]
    public float flushIntervalSeconds = 1.0f;

    [Tooltip("Also print debug output to Console.")]
    public bool debugLog = false;

    private StreamWriter writer;
    private string fullPath;
    private float lastFlushTime = 0f;
    private IEmotionSource source;

    void Start()
    {
        // Auto-find source if not manually assigned
        if (emotionSource == null)
        {
            emotionSource = FindObjectOfType<MonoBehaviour>(includeInactive: true) as MonoBehaviour;
            if (emotionSource != null && emotionSource is IEmotionSource)
            {
                source = (IEmotionSource)emotionSource;
                Debug.Log($"[VALENCE LOGGER] Automatically found emotion source: {emotionSource.GetType().Name}");
            }
            else
            {
                Debug.LogWarning("[VALENCE LOGGER] No valid emotion source found in scene.");
                emotionSource = null;
            }
        }
        else
        {
            source = emotionSource as IEmotionSource;
            if (source == null)
            {
                Debug.LogError("[VALENCE LOGGER] Assigned component does not implement IEmotionSource interface.");
                emotionSource = null;
            }
        }

        // Build unique CSV file path
        string sceneName = SceneManager.GetActiveScene().name;
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string folderPath = Path.Combine(Application.persistentDataPath, "EmotionLogs");

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string fileName = $"{baseFileName}_{sceneName}_{timestamp}.csv";
        fullPath = Path.Combine(folderPath, fileName);

        // Create CSV header
        try
        {
            writer = new StreamWriter(fullPath, append: false);
            writer.AutoFlush = false;
            writer.WriteLine("timestamp_seconds,datetime,scene,valence,arousal,category,frame");
            writer.Flush();
            lastFlushTime = Time.realtimeSinceStartup;
            Debug.Log($"[VALENCE LOGGER] Logging to: {fullPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[VALENCE LOGGER] Failed to open log file: {e.Message}");
            writer = null;
        }
    }

    void Update()
    {
        if (writer == null) return;

        float t = Time.time;
        string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        string scene = SceneManager.GetActiveScene().name;

        float val = float.NaN, aro = float.NaN;
        string category = "Unknown";

        if (source != null)
        {
            try
            {
                (val, aro) = source.GetValenceArousal();
                category = GetEmotionCategory(val, aro);
            }
            catch (Exception e)
            {
                Debug.LogError($"[VALENCE LOGGER] Error reading emotion data: {e.Message}");
            }
        }

        int frame = Time.frameCount;
        string line = $"{t:F3},{dt},{scene},{val:F6},{aro:F6},{category},{frame}";
        writer.WriteLine(line);

        if (debugLog)
            Debug.Log($"[VALENCE LOGGER] {line}");

        // Flush periodically
        if (Time.realtimeSinceStartup - lastFlushTime >= flushIntervalSeconds)
        {
            writer.Flush();
            lastFlushTime = Time.realtimeSinceStartup;
        }
    }

    private string GetEmotionCategory(float val, float aro)
    {
        if (float.IsNaN(val) || float.IsNaN(aro)) return "Unknown";
        if (val >= 0 && aro >= 0) return "HighV_HighA";
        if (val >= 0 && aro < 0) return "HighV_LowA";
        if (val < 0 && aro >= 0) return "LowV_HighA";
        if (val < 0 && aro < 0) return "LowV_LowA";
        return "Unknown";
    }

    void OnApplicationQuit() => CloseWriter();
    void OnDestroy() => CloseWriter();

    private void CloseWriter()
    {
        if (writer != null)
        {
            try
            {
                writer.Flush();
                writer.Close();
                writer = null;
                Debug.Log($"[VALENCE LOGGER] Log saved successfully at: {fullPath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[VALENCE LOGGER] Error closing writer: {e.Message}");
            }
        }
    }
}
