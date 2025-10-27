using UnityEngine;
using Unity.Barracuda;
using System;
using System.IO;
using System.Linq;

// Ensure IEmotionSource.cs exists separately
// public interface IEmotionSource { (float valence, float arousal) GetValenceArousal(); }

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
    public OVRFaceExpressions faceExpressions;   // drag the component from your avatar here

    [Header("Feature Mapping")]
    [Tooltip("List of expressions to feed into model, in same order used during training.")]
    public OVRFaceExpressions.FaceExpression[] featureExpressions;

    [Header("Output (Read Only)")]
    [SerializeField] private float valence;
    [SerializeField] private float arousal;

    public float Valence => valence;
    public float Arousal => arousal;

    public (float valence, float arousal) GetValenceArousal() => (valence, arousal);

    void Start()
    {
        if (faceExpressions == null)
        {
            Debug.LogError("[ONNXEmotionPredictor] OVRFaceExpressions not assigned!");
            enabled = false;
            return;
        }

        // Load ONNX model
        runtimeModel = ModelLoader.Load(onnxModelAsset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);

        // Load normalization stats
        meanValues = LoadTextFile(meanFile);
        scaleValues = LoadTextFile(scaleFile);

        if (featureExpressions.Length != meanValues.Length)
            Debug.LogWarning($"[ONNXEmotionPredictor] Feature count mismatch: expressions={featureExpressions.Length}, mean={meanValues.Length}");

        Debug.Log($"[ONNXEmotionPredictor] Initialized with {featureExpressions.Length} features.");
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
                inputVec[i] = Mathf.Clamp01(w / 100f); // convert Meta’s 0–100 range to 0–1
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

        valence = output[0];
        arousal = output[1];
    }

    private float[] LoadTextFile(TextAsset txt)
    {
        return txt.text
                  .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                  .Select(s => float.Parse(s.Trim()))
                  .ToArray();
    }
}
