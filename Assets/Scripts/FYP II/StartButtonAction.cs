using UnityEngine;
using Oculus.Interaction;   // for InteractableUnityEventWrapper
using UnityEngine.Events;

public class StartButtonAction : MonoBehaviour
{
    [Header("References")]
    public GameManager gameManager; // assign in inspector
    public InteractableUnityEventWrapper pokeInteractableWrapper;

    [Header("Optional Audio")]
    public AudioClip buttonClickSound;
    private AudioSource audioSource;

    void Start()
    {
        // Prepare sound
        if (buttonClickSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = buttonClickSound;
        }

        // Link poke button event
        if (pokeInteractableWrapper != null)
        {
            pokeInteractableWrapper.WhenSelect.AddListener(OnButtonPressed);
        }
        else
        {
            Debug.LogError("[StartButtonAction] Missing InteractableUnityEventWrapper!");
        }
    }

    private void OnButtonPressed()
    {
        Debug.Log("[StartButtonAction] Start Button Pressed!");

        // Play sound
        if (audioSource && buttonClickSound)
            audioSource.Play();

        // Start game through GameManager
        if (gameManager != null)
        {
            gameManager.StartGame();
        }
        else
        {
            Debug.LogError("[StartButtonAction] GameManager not assigned!");
        }
    }
}
