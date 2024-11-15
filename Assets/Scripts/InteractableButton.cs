using UnityEngine;
using Oculus.Interaction;  // For PokeInteractable
using UnityEngine.Events;   // For Unity Events

public class InteractableButton : MonoBehaviour
{
    // Reference to the existing RockPaperScissors script
    public RockPaperScissors gameScript;

    // Optional: Sound effect for button press
    public AudioClip buttonClickSound;
    private AudioSource audioSource;

    // Reference to the PokeInteractable component (set this in the Inspector)
    public InteractableUnityEventWrapper pokeInteractableButtonWrapper;

    void Start()
    {
        // Add an AudioSource if a sound effect is set
        if (buttonClickSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = buttonClickSound;
        }

        // Check if the InteractableUnityEventWrapper is assigned
        if (pokeInteractableButtonWrapper != null)
        {
            // Subscribe to the WhenSelect event from the InteractableUnityEventWrapper
            pokeInteractableButtonWrapper.WhenSelect.AddListener(OnButtonPressed);
        }
        else
        {
            Debug.LogError("InteractableUnityEventWrapper component is missing!");
        }
    }

    // Trigger the action when the button is pressed (poke interaction)
    public void OnButtonPressed()
    {
        Debug.Log("Button Pressed!"); // Confirm the button was pressed

        // Play the button click sound if available
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.Play();
        }

        // Perform the action based on the game state
        if (gameScript != null)
        {
            if (gameScript.IsGameActive()) // If the game is active, reset it
            {
                gameScript.ResetGame();
            }
            else // If the game is inactive, start a new round
            {
                gameScript.StartNewRound();
            }
        }
        else
        {
            Debug.LogError("GameScript reference is missing! Please assign the RockPaperScissors script.");
        }
    }
}
