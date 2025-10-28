using UnityEngine;
using UnityEngine.Rendering;

public class ShadowAndFog_Sad : MonoBehaviour
{
    [Tooltip("Assign a soft/bright skybox material here")]
    public Material sadSkybox;

    [Tooltip("Reference to the default skybox material so OnDisable can revert to default")]
    public Material defaultSkybox;

    private bool applied = false;

    void OnEnable()
    {
        if (applied) return;

        // Apply bright-but-subtle-sad environment settings
        RenderSettings.skybox = sadSkybox;
        RenderSettings.subtractiveShadowColor = new Color(0.4627451f, 0.5411765f, 0.6784314f); // soft blue-ish (#768AAE)
        RenderSettings.ambientMode = AmbientMode.Skybox;
        RenderSettings.ambientIntensity = 0.85f; // slightly softer than default
        RenderSettings.reflectionIntensity = 1f;
        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
        RenderSettings.defaultReflectionResolution = 128;

        // Gentle fog for softness (low density)
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.75f, 0.80f, 0.85f); // soft bluish haze
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.05f;

        applied = true;
    }

    void OnDisable()
    {
        if (!applied) return;

        // Revert to default settings as defined in DefaultLighting
        RenderSettings.skybox = defaultSkybox;
        RenderSettings.subtractiveShadowColor = new Color(0.4196079f, 0.4784314f, 0.6313726f); // #6B7AA1
        RenderSettings.ambientMode = AmbientMode.Skybox;
        RenderSettings.ambientIntensity = 1f;
        RenderSettings.reflectionIntensity = 1f;
        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
        RenderSettings.defaultReflectionResolution = 128;
        RenderSettings.fog = false;

        applied = false;
    }
}
