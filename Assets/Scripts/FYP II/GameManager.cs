using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelManager levelManager;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (levelManager == null)
        {
            levelManager = FindObjectOfType<LevelManager>();
            if (levelManager == null)
                Debug.LogError("[GameManager] LevelManager not found!");
        }
    }

    public void StartGame()
    {
        if (levelManager != null)
            levelManager.LoadLevel(1);  // Load Level01
        else
            Debug.LogError("[GameManager] Cannot start game — LevelManager missing!");
    }
}
