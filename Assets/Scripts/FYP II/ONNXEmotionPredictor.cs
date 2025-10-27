using UnityEngine;
using Unity.Barracuda;
using System;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class ONNXEmotionPredictor : MonoBehaviour, IEmotionSource
{
    [Header("Model & Normalization Files")]
    public NNModel onnxModelAsset;
    public TextAsset meanFile;
    public TextAsset scaleFile;

    private Model runtimeModel;
    private IWorker worker;
    private float[] meanValues;
    private float[] scaleValues;

    [Header("Face Tracking Source (Meta Quest)")]
    [Tooltip("If not assigned, the script will auto-find OVRFaceExpressions on scene load.")]
    public OVRFaceExpressions faceExpressions;

    [Header("Feature Mapping")]
    [Tooltip("List of expressions to feed into model, in same order used during training.")]
    public OVRFaceExpressions.FaceExpression[] featureExpressions;

    [Header("Output (Read Only)")]
    [SerializeField] private float valence;
    [SerializeField] private float arousal;
    [SerializeField] private string emotionCategory;
    [SerializeField] private string currentLevel;
    [SerializeField] private string timestamp;

    public float Valence => valence;
    public float Arousal => arousal;
    public string EmotionCategory => emotionCategory;
    public string LevelName => currentLevel;
    public string Timestamp => timestamp;

    public (float valence, float arousal) GetValenceArousal() => (valence, arousal);

    void Start()
    {
        // Auto-detect current level and timestamp for logging
        currentLevel = SceneManager.GetActiveScene().name;
        timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Auto-find OVRFaceExpressions if not assigned
        if (faceExpressions == null)
        {
            faceExpressions = FindObjectOfType<OVRFaceExpressions>();
            if (faceExpressions == null)
            {
                Debug.LogError("[ONNXEmotionPredictor] No OVRFaceExpressions found in scene!");
                enabled = false;
                return;
            }
            else
            {
                Debug.Log("[ONNXEmotionPredictor] Auto-assigned OVRFaceExpressions.");
            }
        }

        // Load ONNX model
        runtimeModel = ModelLoader.Load(onnxModelAsset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);

        // Load normalization stats
        meanValues = LoadTextFile(meanFile);
        scaleValues = LoadTextFile(scaleFile);

        if (featureExpressions.Length != meanValues.Length)
            Debug.LogWarning($"[ONNXEmotionPredictor] Feature count mismatch: expressions={featureExpressions.Length}, mean={meanValues.Length}");

        Debug.Log($"[ONNXEmotionPredictor] Initialized with {featureExpressions.Length} features in level '{currentLevel}'.");
    }

    void OnDestroy()
    {
        worker?.Dispose();
    }

    void Update()
    {
        if (faceExpressions == null || !faceExpressions.ValidExpressions) return;

        // Build input vector from tracked blendshapes
        float[] inputVec = new float[featureExpressions.Length];
        for (int i = 0; i < featureExpressions.Length; i++)
        {
            if (faceExpressions.TryGetFaceExpressionWeight(featureExpressions[i], out float w))
                inputVec[i] = Mathf.Clamp01(w / 100f);
            else
                inputVec[i] = 0f;
        }

        // Normalize
        float[] normalized = new float[inputVec.Length];
        for (int i = 0; i < inputVec.Length; i++)
        {
            float scale = (scaleValues.Length > i && scaleValues[i] != 0f) ? scaleValues[i] : 1f;
            float mean = (meanValues.Length > i) ? meanValues[i] : 0f;
            normalized[i] = (inputVec[i] - mean) / scale;
        }

        // Run inference
        using var tensor = new Tensor(1, normalized.Length, normalized);
        worker.Execute(tensor);
        using var output = worker.PeekOutput();

        // Safe access to ONNX output values
        Debug.Log($"[ONNXEmotionPredictor] Output tensor length: {output.length}");

        if (output.length >= 2)
        {
            valence = output[0];
            arousal = output[1];
        }
        else if (output.length == 1)
        {
            valence = output[0];
            arousal = 0f;
            Debug.LogWarning("[ONNXEmotionPredictor] Model output only one value — assuming Valence only.");
        }
        else
        {
            valence = 0f;
            arousal = 0f;
            Debug.LogError("[ONNXEmotionPredictor] Model produced no output values!");
        }

        Debug.Log($"[ONNXEmotionPredictor] Valence={valence:F3}, Arousal={arousal:F3}");

        emotionCategory = ClassifyEmotion(valence, arousal);
    }

    private string ClassifyEmotion(float v, float a)
    {
        if (v >= 0 && a >= 0) return "High Valence, High Arousal (HVHA)";
        else if (v >= 0 && a < 0) return "High Valence, Low Arousal (HVLA)";
        else if (v < 0 && a >= 0) return "Low Valence, High Arousal (LVHA)";
        else return "Low Valence, Low Arousal (LVLA)";
    }

    private float[] LoadTextFile(TextAsset txt)
    {
        return txt.text
            .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => float.Parse(s.Trim()))
            .ToArray();
    }
}
