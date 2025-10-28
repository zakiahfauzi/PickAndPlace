using UnityEngine;
using UnityEngine.Rendering;

public class DefaultLighting : MonoBehaviour
{
    [Tooltip("Assign the Default-Skybx material here")]
    public Material defaultSkybox; // "Default-Skybx"

    private bool applied = false;

    void OnEnable()
    {
        if (applied) return;

        ApplyDefaultSettings();
        applied = true;
    }

    void OnDisable()
    {
        if (!applied) return;

        // Re-apply default settings when this mode is disabled
        ApplyDefaultSettings();
        applied = false;
    }

    private void ApplyDefaultSettings()
    {
        RenderSettings.skybox = defaultSkybox;
        RenderSettings.subtractiveShadowColor = new Color(0.4196079f, 0.4784314f, 0.6313726f); // #6B7AA1
        RenderSettings.ambientMode = AmbientMode.Skybox;
        RenderSettings.ambientIntensity = 1f;
        RenderSettings.reflectionIntensity = 1f;
        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
        RenderSettings.defaultReflectionResolution = 128;
        RenderSettings.fog = false;
    }
}
