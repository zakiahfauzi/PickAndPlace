using UnityEngine;
using UnityEngine.Rendering;

public class ShadowAndFog_Stressed : MonoBehaviour
{
    [Header("Skybox Settings")]
    [Tooltip("Assign a dark or horror-themed skybox (optional). If none, scene will be dark.")]
    public Material stressedSkybox;

    [Tooltip("Default skybox material to revert to when disabled.")]
    public Material defaultSkybox;

    private bool applied = false;

    void OnEnable()
    {
        if (applied) return;

        // --- SKYBOX ---
        if (stressedSkybox != null)
            RenderSettings.skybox = stressedSkybox;
        else
            RenderSettings.skybox = null; // No skybox = pure dark background

        // --- LIGHTING ---
        RenderSettings.subtractiveShadowColor = new Color(0.05f, 0.05f, 0.05f);  // near-black shadows
        RenderSettings.ambientMode = AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.05f, 0.02f, 0.02f);  // dark red tint
        RenderSettings.ambientEquatorColor = new Color(0.02f, 0.01f, 0.01f);
        RenderSettings.ambientGroundColor = new Color(0.01f, 0.005f, 0.005f);
        RenderSettings.ambientIntensity = 0.3f;

        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
        RenderSettings.reflectionIntensity = 0.2f;

        // --- FOG ---
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.25f; // thicker fog for tension
        RenderSettings.fogColor = new Color(0.12f, 0.02f, 0.02f); // dark red-black fog

        // --- FLARE / HALO EFFECTS ---
        RenderSettings.haloStrength = 0.3f;
        RenderSettings.flareFadeSpeed = 0.5f;
        RenderSettings.flareStrength = 0.7f;

        // --- DIRECTIONAL LIGHT ADJUSTMENT (optional but adds realism) ---
        Light mainLight = RenderSettings.sun;
        if (mainLight != null)
        {
            mainLight.color = new Color(1f, 0.4f, 0.3f);  // red-orange tint
            mainLight.intensity = 0.2f;
            mainLight.shadows = LightShadows.Soft;
        }

        // --- OPTIONAL: Change ambient audio or tone mapping in another script ---

        applied = true;
    }

    void OnDisable()
    {
        if (!applied) return;

        // Revert to default environment
        RenderSettings.skybox = defaultSkybox;
        RenderSettings.subtractiveShadowColor = new Color(0.4196079f, 0.4784314f, 0.6313726f);
        RenderSettings.ambientMode = AmbientMode.Skybox;
        RenderSettings.ambientIntensity = 1f;
        RenderSettings.reflectionIntensity = 1f;
        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
        RenderSettings.fog = false;

        RenderSettings.haloStrength = 0f;
        RenderSettings.flareFadeSpeed = 1f;
        RenderSettings.flareStrength = 0f;

        Light mainLight = RenderSettings.sun;
        if (mainLight != null)
        {
            mainLight.color = Color.white;
            mainLight.intensity = 1f;
        }

        applied = false;
    }
}
