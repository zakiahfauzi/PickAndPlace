using System;
using System.IO;
using UnityEngine;

public class ValenceArousalLogger : MonoBehaviour
{
    [Tooltip("Component which implements IEmotionSource (returns valence, arousal).")]
    public MonoBehaviour emotionSource; // must implement IEmotionSource

    [Tooltip("Base filename (timestamp will be added automatically).")]
    public string baseFileName = "valence_arousal_log";

    [Tooltip("Minimum seconds between writes to disk (throttle). Set to 0 to write every frame.")]
    public float flushIntervalSeconds = 1.0f;

    [Tooltip("Also print debug output to Console")]
    public bool debugLog = false;

    private StreamWriter writer;
    private string fullPath;
    private float lastFlushTime = 0f;
    private IEmotionSource source;

    void Start()
    {
        // Validate emotion source
        if (emotionSource == null)
        {
            Debug.LogWarning("[VALENCE LOGGER] No emotionSource assigned. Add a component that implements IEmotionSource.");
        }
        else
        {
            source = emotionSource as IEmotionSource;
            if (source == null)
            {
                Debug.LogError("[VALENCE LOGGER] Assigned emotionSource does not implement IEmotionSource interface.");
                emotionSource = null;
            }
        }

        // Create unique timestamped filename
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = $"{baseFileName}_{timestamp}.csv";
        fullPath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            writer = new StreamWriter(fullPath, append: false); // always new file
            writer.AutoFlush = false;
            writer.WriteLine("timestamp_seconds,datetime,valence,arousal,frame");
            writer.Flush();
            lastFlushTime = Time.realtimeSinceStartup;
            Debug.Log($"[VALENCE LOGGER] Logging to: {fullPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[VALENCE LOGGER] Failed to open log file: {e}");
            writer = null;
        }
    }

    void Update()
    {
        if (writer == null) return;

        float t = Time.time;
        string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

        float val = float.NaN, aro = float.NaN;

        if (source != null)
        {
            try
            {
                (val, aro) = source.GetValenceArousal();
            }
            catch (Exception e)
            {
                Debug.LogError($"[VALENCE LOGGER] Error getting values from source: {e}");
            }
        }

        int frame = Time.frameCount;

        // Write CSV line
        string line = $"{t:F3},{dt},{val:F6},{aro:F6},{frame}";
        writer.WriteLine(line);

        if (debugLog) Debug.Log($"[VALENCE LOGGER] {line}");

        // Periodic flush
        if (Time.realtimeSinceStartup - lastFlushTime >= flushIntervalSeconds)
        {
            writer.Flush();
            lastFlushTime = Time.realtimeSinceStartup;
        }
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
                Debug.Log($"[VALENCE LOGGER] Log saved: {fullPath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[VALENCE LOGGER] Error closing writer: {e}");
            }
        }
    }

    public void ForceFlush()
    {
        if (writer != null)
        {
            writer.Flush();
            lastFlushTime = Time.realtimeSinceStartup;
            Debug.Log("[VALENCE LOGGER] Forced flush.");
        }
    }
}
