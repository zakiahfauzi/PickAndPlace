using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class CalmSadAtmosphere : MonoBehaviour
{
    [Header("Light Settings")]
    public Light mainLight; // Assign your main Directional Light
    [Tooltip("Shadow tint color (hex) for cool mood")]
    public string shadowHex = "708090"; // soft slate blue-gray
    [Range(0f, 1f)]
    public float ambientIntensity = 0.55f; // brighter ambient to keep scene visible

    [Header("Fog Settings")]
    public string fogHex = "7F8C99"; // light misty blue-gray
    public FogMode fogMode = FogMode.ExponentialSquared;
    [Range(0f, 1f)]
    public float fogDensity = 0.10f; // gentle fog instead of thick

    void OnValidate() => ApplyMood();
    void Start() => ApplyMood();

    void ApplyMood()
    {
        ApplyAmbientAndShadows();
        ApplyFog();
        ConfigureLight();
    }

    void ApplyAmbientAndShadows()
    {
        Color shadowColor = HexToColor(shadowHex);
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = shadowColor * ambientIntensity;
    }

    void ConfigureLight()
    {
        if (mainLight == null) return;

        // Keep it naturally bright but cool-toned
        mainLight.color = new Color(0.8f, 0.86f, 0.92f); // soft cold daylight
        mainLight.intensity = 1.0f; // maintain good illumination
        mainLight.shadows = LightShadows.Soft;
        mainLight.shadowStrength = 0.85f;
        mainLight.lightmapBakeType = LightmapBakeType.Mixed;
    }

    void ApplyFog()
    {
        Color fogColor = HexToColor(fogHex);
        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogMode = fogMode;
        RenderSettings.fogDensity = fogDensity;
    }

    Color HexToColor(string hex)
    {
        if (string.IsNullOrEmpty(hex)) return Color.gray;
        hex = hex.TrimStart('#');

        if (hex.Length != 6)
        {
            Debug.LogWarning("Invalid hex color, using gray instead.");
            return Color.gray;
        }

        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        return new Color(r / 255f, g / 255f, b / 255f);
    }
}
