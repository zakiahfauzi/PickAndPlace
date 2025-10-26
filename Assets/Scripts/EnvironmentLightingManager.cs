using UnityEngine;
using UnityEngine.Rendering;

public static class EnvironmentLightingManager
{
    private static Material backupSkybox;
    private static Color backupAmbientLight;
    private static AmbientMode backupAmbientMode;
    private static float backupAmbientIntensity;
    private static Light backupSun;
    private static Color backupSubtractiveShadowColor;

    private static bool backupFogEnabled;
    private static Color backupFogColor;
    private static FogMode backupFogMode;
    private static float backupFogDensity;

    private static bool isBackupSaved = false;

    public static void SaveGlobalSettings()
    {
        if (isBackupSaved) return;

        backupSkybox = RenderSettings.skybox;
        backupAmbientLight = RenderSettings.ambientLight;
        backupAmbientMode = RenderSettings.ambientMode;
        backupAmbientIntensity = RenderSettings.ambientIntensity;
        backupSun = RenderSettings.sun;
        backupSubtractiveShadowColor = RenderSettings.subtractiveShadowColor;

        backupFogEnabled = RenderSettings.fog;
        backupFogColor = RenderSettings.fogColor;
        backupFogMode = RenderSettings.fogMode;
        backupFogDensity = RenderSettings.fogDensity;

        isBackupSaved = true;
    }

    public static void RestoreGlobalSettings()
    {
        if (!isBackupSaved) return;

        RenderSettings.skybox = backupSkybox;
        RenderSettings.ambientLight = backupAmbientLight;
        RenderSettings.ambientMode = backupAmbientMode;
        RenderSettings.ambientIntensity = backupAmbientIntensity;
        RenderSettings.sun = backupSun;
        RenderSettings.subtractiveShadowColor = backupSubtractiveShadowColor;

        RenderSettings.fog = backupFogEnabled;
        RenderSettings.fogColor = backupFogColor;
        RenderSettings.fogMode = backupFogMode;
        RenderSettings.fogDensity = backupFogDensity;

        isBackupSaved = false;
    }
}
