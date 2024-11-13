using UnityEngine;
using TMPro;  // For TextMesh Pro UI components

public class CubePlacementChecker : MonoBehaviour
{
    public GameObject[] cubes;  // Array to hold the cubes that the player will move
    public TextMeshProUGUI successMessage; // TextMesh Pro UI for success message

    private int cubesInTargetZone = 0; // Counter to track cubes in the target area

    // Called when the game starts
    private void Start()
    {
        // Initial instruction message
        successMessage.text = "Move all the cubes to the target area!";
    }

    // Called when another collider enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is one of the cubes
        for (int i = 0; i < cubes.Length; i++)
        {
            if (other.gameObject == cubes[i])
            {
                cubesInTargetZone++;
                CheckSuccess();
                break;
            }
        }
    }

    // Called when another collider exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        // Check if the collider is one of the cubes
        for (int i = 0; i < cubes.Length; i++)
        {
            if (other.gameObject == cubes[i])
            {
                cubesInTargetZone--;
                CheckSuccess();
                break;
            }
        }
    }

    // Check if all cubes are in the target zone
    private void CheckSuccess()
    {
        if (cubesInTargetZone == cubes.Length)
        {
            // Display success message when all cubes are in the target area
            successMessage.text = "Success! All cubes are in the target area!";
        }
    }
}
