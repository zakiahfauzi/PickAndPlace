using UnityEngine;

public class TestEmotionSource : MonoBehaviour, IEmotionSource
{
    [Tooltip("Oscillation speed for testing")]
    public float speed = 0.5f;

    [Tooltip("Valence amplitude (max absolute value)")]
    public float valenceAmp = 1.0f;

    [Tooltip("Arousal amplitude (max absolute value)")]
    public float arousalAmp = 1.0f;

    public (float valence, float arousal) GetValenceArousal()
    {
        float t = Time.time * speed;
        float val = Mathf.Sin(t) * valenceAmp;
        float aro = Mathf.Cos(t) * arousalAmp;
        return (val, aro);
    }
}
