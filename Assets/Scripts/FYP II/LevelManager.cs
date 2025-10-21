using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Ordered Level Names")]
    public string[] levelScenes = { "MainMenu", "Level01", "Level02", "Level03" };
    public Transform playerRig;  // your OVRCameraRig root
    private int currentIndex = -1;
    private string activeScene;

    public void LoadLevel(int index)
    {
        if (index < 0 || index >= levelScenes.Length) return;

        Debug.Log($"[LevelManager] LoadLevel() called with index: {index}");

        string newScene = levelScenes[index];
        SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive)
            .completed += (op) =>
            {
                activeScene = newScene;
                currentIndex = index;
                Debug.Log($"[LevelManager] Successfully loaded {newScene}");

                // Move player to spawn
                var spawn = GameObject.FindWithTag("PlayerSpawn");
                if (spawn && playerRig)
                {
                    playerRig.position = spawn.transform.position;
                    playerRig.rotation = spawn.transform.rotation;
                    Debug.Log("[LevelManager] Player moved to spawn point.");
                }
                else
                {
                    Debug.LogWarning("[LevelManager] No PlayerSpawn found or PlayerRig not assigned!");
                }

                // 🔥 Unload previous scene (MainMenu)
                UnloadPreviousScene(newScene);
            };
    }

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

    public void LoadNextLevel()
    {
        LoadLevel(currentIndex + 1);
    }
}
