using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Ordered Level Names")]
    public string[] levelScenes = { "MainMenu", "Level01", "Level02", "Level03" };
    [Header("Reference to your OVRCameraRig root")]
    public Transform playerRig;  // Persistent player rig (OVRCameraRig)
    
    private int currentIndex = -1;
    private string activeScene;

    // ✅ Main entry point for loading a level by index
    public void LoadLevel(int index)
    {
        if (index < 0 || index >= levelScenes.Length)
        {
            Debug.LogWarning("[LevelManager] Invalid level index!");
            return;
        }

        Debug.Log($"[LevelManager] LoadLevel() called with index: {index}");
        string newScene = levelScenes[index];

        // Load scene additively so playerRig persists
        SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive).completed += (op) =>
        {
            activeScene = newScene;
            currentIndex = index;
            Debug.Log($"[LevelManager] Successfully loaded {newScene}");

            // Move player to correct spawn in that scene
            StartCoroutine(MovePlayerToSpawn(newScene));

            // Unload previous scenes except the new one
            UnloadPreviousScene(newScene);
        };
    }

    // ✅ Ensures we only look for spawn points inside the newly loaded scene
    private IEnumerator MovePlayerToSpawn(string sceneName)
    {
        // Wait one frame so all scene objects initialize
        yield return null;

        var scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.isLoaded)
        {
            Debug.LogWarning("[LevelManager] Scene not fully loaded yet!");
            yield break;
        }

        GameObject spawn = null;

        // Search only in the root objects of the new scene
        foreach (var rootObj in scene.GetRootGameObjects())
        {
            // Try to find any child tagged PlayerSpawn
            var potentialSpawn = rootObj.GetComponentInChildren<Transform>();
            if (potentialSpawn != null && potentialSpawn.CompareTag("PlayerSpawn"))
            {
                spawn = potentialSpawn.gameObject;
                break;
            }
        }

        if (spawn != null && playerRig != null)
        {
            // Move the player rig to the new spawn
            playerRig.position = spawn.transform.position;
            playerRig.rotation = spawn.transform.rotation;

            // Reset OVR tracking offset to avoid cumulative drift
            Transform trackingSpace = playerRig.Find("TrackingSpace");
            if (trackingSpace != null)
                trackingSpace.localPosition = Vector3.zero;

            Debug.Log($"[LevelManager] Player moved to spawn in scene: {sceneName}");
        }
        else
        {
            Debug.LogWarning("[LevelManager] PlayerSpawn not found or playerRig not assigned!");
        }
    }

    //  Clean up old scenes once a new one is loaded
    private void UnloadPreviousScene(string newScene)
    {
        foreach (var scene in levelScenes)
        {
            if (scene != newScene && SceneManager.GetSceneByName(scene).isLoaded)
            {
                SceneManager.UnloadSceneAsync(scene);
                Debug.Log($"[LevelManager] Unloaded previous scene: {scene}");
            }
        }
    }

    //  Shortcut to move to next level
    public void LoadNextLevel()
    {
        LoadLevel(currentIndex + 1);
    }
}
