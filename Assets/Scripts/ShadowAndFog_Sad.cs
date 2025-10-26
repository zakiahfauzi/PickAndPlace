using UnityEngine;
using UnityEngine.Rendering;

public class ShadowAndFog_Sad : MonoBehaviour
{
    [Header("Skybox")]
    public Material sadSkybox; // Optional, assign if you have one
    public Light mainLight;    // Assign main directional light in Inspector
    private bool applied = false;

    void OnEnable()
    {
        if (applied) return;
        EnvironmentLightingManager.SaveGlobalSettings();

        if (sadSkybox != null) RenderSettings.skybox = sadSkybox;
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.44f, 0.50f, 0.56f) * 0.55f; // #708090 * intensity
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.50f, 0.55f, 0.60f); // #7F8C99
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.10f;

        if (mainLight != null)
        {
            mainLight.color = new Color(0.8f, 0.86f, 0.92f);
            mainLight.intensity = 1.0f;
            mainLight.shadows = LightShadows.Soft;
            mainLight.shadowStrength = 0.85f;
            mainLight.lightmapBakeType = LightmapBakeType.Mixed;
        }

        applied = true;
    }

    void OnDisable()
    {
        if (!applied) return;
        EnvironmentLightingManager.RestoreGlobalSettings();
        applied = false;
    }
}
