using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class Level01ToLevel02 : MonoBehaviour
{
    [Header("Completion Settings")]
    public int totalCubes = 4;

    [Header("UI")]
    public GameObject instructionUI;    // Panel for both instructions & success
    public TMP_Text instructionText;    // TMP Text inside the panel

    [Header("Game Manager")]
    public GameManager gameManager;     // leave null to auto-find

    private HashSet<GameObject> cubesInTarget = new HashSet<GameObject>();
    private bool levelCompleted = false;
    private float startTime;

    void Start()
    {
        startTime = Time.time;

        if (instructionUI) instructionUI.SetActive(true);
        if (instructionText) instructionText.text = "Move all cubes to the target area!";

        // Auto-find persistent GameManager
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (levelCompleted) return;
        if (other.CompareTag("MovableCube"))
        {
            cubesInTarget.Add(other.gameObject);
            CheckCompletion();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (levelCompleted) return;
        if (other.CompareTag("MovableCube"))
            cubesInTarget.Remove(other.gameObject);
    }

    void CheckCompletion()
    {
        if (levelCompleted) return;

        if (cubesInTarget.Count >= totalCubes)
        {
            levelCompleted = true;
            float timeTaken = Time.time - startTime;
            SaveTime(timeTaken);
            StartCoroutine(ShowSuccessAndLoadNext(timeTaken));
        }
    }

    void SaveTime(float seconds)
    {
        string ts = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string path = Path.Combine(Application.persistentDataPath, $"Level01_{ts}.txt");
        string content = $"Level01 completed in {seconds:F2} seconds ({seconds/60f:F2} minutes) | {ts}";
        try
        {
            File.WriteAllText(path, content);
            Debug.Log($"[Level01ToLevel02] Saved time to: {path}");
        }
        catch
        {
            Debug.LogError("[Level01ToLevel02] Failed to save time.");
        }
    }

    IEnumerator ShowSuccessAndLoadNext(float seconds)
    {
        // Display countdown on the same panel
        if (instructionText != null)
        {
            int countdown = 3;
            while (countdown > 0)
            {
                instructionText.text = $" Level Complete!\nTime: {seconds:F2}s\nNext level in {countdown}...";
                yield return new WaitForSeconds(1f);
                countdown--;
            }
            instructionText.text = " Loading next level...";
            yield return new WaitForSeconds(0.5f);
        }

        // Auto-find GameManager/LevelManager if missing
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null && gameManager.levelManager == null)
            gameManager.levelManager = FindObjectOfType<LevelManager>();

        if (gameManager != null && gameManager.levelManager != null)
        {
            gameManager.levelManager.LoadNextLevel();  // Automatically loads Level02
        }
        else
        {
            Debug.LogError("[Level01ToLevel02] Cannot load next level. Missing GameManager or LevelManager!");
        }
    }
}
