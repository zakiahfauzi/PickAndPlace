using UnityEngine;
using UnityEngine.Rendering; // for AmbientMode

[ExecuteAlways]
public class ShadowAndFogController : MonoBehaviour
{
    [Header("Lights")]
    public Light directionalLight; // assign your main directional light (set to Realtime)

    [Header("Shadow Tint (approx)")]
    [Tooltip("Hex color for shadow tint (e.g. 0x899BA4)")]
    public string shadowHex = "899BA4";
    [Range(0f, 1f)]
    public float shadowAmbientIntensity = 0.6f; // how strong the shadow tint (affects ambient lighting)

    [Header("Fog")]
    [Tooltip("Hex color for fog (e.g. 0x808080)")]
    public string fogHex = "808080";
    public FogMode fogMode = FogMode.ExponentialSquared;
    public float fogDensity = 0.15f;

    void OnValidate()
    {
        ApplyAllSettings();
    }

    void Start()
    {
        ApplyAllSettings();
    }

    public void ApplyAllSettings()
    {
        ApplyShadowTint();
        ConfigureDirectionalLight();
        ConfigureFog();
    }

    void ApplyShadowTint()
    {
        Color shadowColor = HexToColor(shadowHex);
        // Use ambient light as an approximation for shadow tint (Built-in RP)
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = shadowColor * shadowAmbientIntensity;
        
        // If you want a stronger "shadow color" effect you can also set a global shader color
        // Shader.SetGlobalColor("_CustomShadowTint", shadowColor); // requires custom shader support
    }

    void ConfigureDirectionalLight()
    {
        if (directionalLight == null) return;

        directionalLight.lightmapBakeType = LightmapBakeType.Mixed; // optional
        directionalLight.shadows = LightShadows.Soft;
        directionalLight.shadowStrength = 1f; // 0..1
        directionalLight.shadowBias = 0.05f;
        directionalLight.shadowNormalBias = 0.4f;
        directionalLight.renderMode = LightRenderMode.ForcePixel; // prefer pixel lights for quality
        directionalLight.lightShadowCasterMode = LightShadowCasterMode.Everything; // Unity 2021+ compatibility
        // Ensure the Light is Realtime or Mixed as desired in the inspector.
    }

    void ConfigureFog()
    {
        Color fogColor = HexToColor(fogHex);
        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogMode = fogMode;
        RenderSettings.fogDensity = fogDensity;
    }

    // Simple hex -> Unity Color (accepts "RRGGBB" or "#RRGGBB")
    Color HexToColor(string hex)
    {
        if (string.IsNullOrEmpty(hex)) return Color.white;
        hex = hex.TrimStart('#');

        if (hex.Length != 6)
        {
            Debug.LogWarning("Hex must be 6 characters (RRGGBB). Falling back to white.");
            return Color.white;
        }

        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        return new Color(r / 255f, g / 255f, b / 255f, 1f);
    }
}
