using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public Button StartButton;
    public Button PlayAgainButton;
    public TextMeshProUGUI firstTo3Text;
    public RockPaperScissors gameController; // Reference to RockPaperScissors script

    void Start()
    {
        // Initialize UI: Show Start, Hide PlayAgain
        StartButton.gameObject.SetActive(true);
        PlayAgainButton.gameObject.SetActive(false);

        // Link button clicks to functions
        StartButton.onClick.AddListener(OnStartGame);
        PlayAgainButton.onClick.AddListener(OnPlayAgain);
    }

    // Called when Start button is pressed
    void OnStartGame()
    {
        StartButton.gameObject.SetActive(false);
        gameController.StartNewRound();
    }

    // Called when Play Again button is pressed
    void OnPlayAgain()
    {
        PlayAgainButton.gameObject.SetActive(false);
        gameController.ResetGame();
        gameController.StartNewRound();
    }

    // Show PlayAgainButton when game ends
    public void ShowPlayAgain()
    {
        PlayAgainButton.gameObject.SetActive(true);
    }
}
