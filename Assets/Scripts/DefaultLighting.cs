using UnityEngine;
using UnityEngine.Rendering;

public class DefaultLighting : MonoBehaviour
{
    [Header("Skybox")]
    public Material defaultSkybox; // Assign "Default-Skybox" material in Inspector
    private bool applied = false;

    void OnEnable()
    {
        if (applied) return;
        EnvironmentLightingManager.SaveGlobalSettings();

        RenderSettings.skybox = defaultSkybox;
        RenderSettings.subtractiveShadowColor = new Color(0.42f, 0.48f, 0.63f); // #6B7AA0
        RenderSettings.ambientMode = AmbientMode.Skybox;
        RenderSettings.ambientIntensity = 1f;
        RenderSettings.fog = false;

        applied = true;
    }

    void OnDisable()
    {
        if (!applied) return;
        EnvironmentLightingManager.RestoreGlobalSettings();
        applied = false;
    }
}
