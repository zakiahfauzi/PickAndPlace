using UnityEngine;
using Oculus.Interaction;  // For poke interaction

public class StartButtonAction : MonoBehaviour
{
    [Header("Game References")]
    public GameManager gameManager;  // Assign in Inspector

    [Header("Sound")]
    public AudioClip buttonClickSound;
    private AudioSource audioSource;

    [Header("Oculus Interaction")]
    public InteractableUnityEventWrapper pokeInteractableButtonWrapper;

    void Start()
    {
        // Add audio source if needed
        if (buttonClickSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = buttonClickSound;
        }

        // Subscribe to Oculus button press
        if (pokeInteractableButtonWrapper != null)
        {
            pokeInteractableButtonWrapper.WhenSelect.AddListener(OnButtonPressed);
        }
        else
        {
            Debug.LogWarning("[StartButtonAction] InteractableUnityEventWrapper is missing!");
        }
    }

    private void OnButtonPressed()
    {
        Debug.Log("[StartButtonAction] Button Pressed!");

        // Play sound if available
        if (audioSource != null && buttonClickSound != null)
            audioSource.Play();

        // Start the game
        if (gameManager != null)
        {
            Debug.Log("[StartButtonAction] Calling GameManager.StartGame()");
            gameManager.StartGame();
        }
        else
        {
            Debug.LogError("[StartButtonAction] GameManager reference missing!");
        }
    }
}
