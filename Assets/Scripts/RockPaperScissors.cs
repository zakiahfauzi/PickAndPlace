using UnityEngine;
using TMPro;  // Import TextMeshPro

public class RockPaperScissors : MonoBehaviour
{
    public TextMeshProUGUI countdownText;  // UI for countdown timer
    public TextMeshProUGUI enemyGestureText;  // UI for enemy's gesture
    public TextMeshProUGUI resultText;  // UI for game result

    public float countdownTime = 3f;  // Countdown duration
    private float countdown;
    private bool gameActive = false;  // To control the game state
    private string[] gestures = { "Rock", "Paper", "Scissors" };  // Array for enemy gestures
    private string enemyGesture;  // Stores enemy's chosen gesture

    // Assume playerGesture is set by another script you wrote
    public string playerGesture;

    void Start()
    {
        countdown = countdownTime;
        StartNewRound();
    }

    void Update()
    {
        if (gameActive)
        {
            RunCountdown();
        }
    }

    // Starts a new game round
    void StartNewRound()
    {
        resultText.text = "";  // Clear previous result
        countdown = countdownTime;  // Reset the countdown
        enemyGesture = gestures[Random.Range(0, gestures.Length)];  // Random enemy gesture
        enemyGestureText.text = "Enemy Gesture: ???";  // Initially hide enemy gesture
        gameActive = true;  // Mark the game as active
    }

    // Handles the countdown timer
    void RunCountdown()
    {
        countdown -= Time.deltaTime;
        countdownText.text = "Time: " + Mathf.Ceil(countdown).ToString();  // Update countdown UI

        if (countdown <= 0)
        {
            countdown = 0;
            gameActive = false;
            ShowResult();  // Countdown finished, show result
        }
    }

    // Compares gestures and shows the result
    void ShowResult()
    {
        // Reveal enemy gesture on the UI
        enemyGestureText.text = "Enemy Gesture: " + enemyGesture;

        // Compare the player gesture with the enemy's gesture
        if (playerGesture == enemyGesture)
        {
            resultText.text = "Draw!";
        }
        else if ((playerGesture == "Rock" && enemyGesture == "Scissors") ||
                 (playerGesture == "Paper" && enemyGesture == "Rock") ||
                 (playerGesture == "Scissors" && enemyGesture == "Paper"))
        {
            resultText.text = "You Win!";
        }
        else
        {
            resultText.text = "You Lose!";
        }

        // Start a new round after 3 seconds
        Invoke("StartNewRound", 3f);  // Restart the game after a delay
    }
}


