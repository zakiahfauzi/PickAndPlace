using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Ordered Level Names")]
    public string[] levelScenes = { "MainMenu", "Level01", "Level02", "Level03" };

    private int currentIndex = -1;
    private string activeScene;

    public void LoadLevel(int index)
    {
        if (index < 0 || index >= levelScenes.Length)
        {
            Debug.LogWarning("[LevelManager] Invalid level index!");
            return;
        }

        Debug.Log($"[LevelManager] Loading level {levelScenes[index]}...");
        SceneManager.LoadScene(levelScenes[index]); // Simple load — not additive
        currentIndex = index;
        activeScene = levelScenes[index];
    }

    public void LoadNextLevel()
    {
        LoadLevel(currentIndex + 1);
    }
}
