using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelManager levelManager;

    private void Awake()
    {
        if (levelManager == null)
        {
            levelManager = FindObjectOfType<LevelManager>();
            if (levelManager == null)
                Debug.LogError("[GameManager] No LevelManager found in scene!");
            else
                Debug.Log("[GameManager] LevelManager found automatically.");
        }
    }

    public void StartGame()
    {
        Debug.Log("[GameManager] StartGame() called.");

        if (levelManager == null)
        {
            Debug.LogError("[GameManager] Cannot start game — LevelManager reference is null!");
            return;
        }

        // Load Level01 (index 1 since MainMenu = 0)
        Debug.Log("[GameManager] Attempting to load Level01 (index 1)...");
        levelManager.LoadLevel(1);
    }
}
