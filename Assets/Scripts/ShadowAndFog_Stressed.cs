using UnityEngine;
using UnityEngine.Rendering;

public class ShadowAndFog_Stressed : MonoBehaviour
{
    [Header("Skybox")]
    public Material stressedSkybox; // Assign "Skybox_Stressed" in Inspector
    private bool applied = false;

    void OnEnable()
    {
        if (applied) return;
        EnvironmentLightingManager.SaveGlobalSettings();

        RenderSettings.skybox = stressedSkybox;
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.54f, 0.61f, 0.65f) * 0.6f; // #899BA4
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.5f, 0.5f, 0.5f);
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.15f;

        applied = true;
    }

    void OnDisable()
    {
        if (!applied) return;
        EnvironmentLightingManager.RestoreGlobalSettings();
        applied = false;
    }
}
