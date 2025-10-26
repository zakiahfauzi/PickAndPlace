using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelManager levelManager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Persist GameManager

        if (levelManager == null)
        {
            levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null)
            {
                DontDestroyOnLoad(levelManager.gameObject); // Persist LevelManager
                Debug.Log("[GameManager] LevelManager found and persisted.");
            }
            else
            {
                Debug.LogError("[GameManager] No LevelManager found in scene!");
            }
        }
    }

    public void StartGame()
    {
        if (levelManager == null)
        {
            // Re-check just in case
            levelManager = FindObjectOfType<LevelManager>();
            if (levelManager == null)
            {
                Debug.LogError("[GameManager] Cannot start game — LevelManager reference is null!");
                return;
            }
        }

        // Load Level01
        levelManager.LoadLevel(1);
    }
}
