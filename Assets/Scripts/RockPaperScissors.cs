using UnityEngine;
using TMPro;
using Oculus.Interaction.Samples;
using System.IO;

public class RockPaperScissors : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI enemyGestureText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI playerGestureText; 
    public TextMeshProUGUI playerScoreText; 
    public TextMeshProUGUI enemyScoreText;
    public TextMeshProUGUI firstTo3Text;

    public float countdownTime = 3f;
    private float countdown;
    private bool gameActive = false;
    private string[] gestures = { "Rock", "Paper", "Scissors" };
    private string enemyGesture;
    private string playerGesture;

    // Reference to hand models
    public GameObject Rock;
    public GameObject Paper;
    public GameObject Scissors;

    // Materials for inactive and active colors
    public Material inactiveMaterial;
    public Material activeMaterial;

    // Score tracking
    private int playerScore = 0;
    private int enemyScore = 0;

    // New sound variables
    public AudioClip countdownClip; // Sound for each countdown tick
    public AudioClip winClip;       // Sound for winning
    public AudioClip loseClip;       // Sound for loosing
    private AudioSource audioSource; // AudioSource component

    // Variable to track the last second for countdown sound
    private int lastCountdownSecond = -1;

    // Variable to track total game time
    private float totalTime = 0f;

    private GameUIController gameUIController;

    void OnEnable()
    {
        // Subscribe to the gesture detection event
        PoseDetection.GestureDetected += OnGestureDetected;
    }

    void OnDisable()
    {
        // Unsubscribe when the script is disabled
        PoseDetection.GestureDetected -= OnGestureDetected;
    }

    void Start()
    {
        countdown = countdownTime;
        UpdateScoreUI();

        // Set up the audio source
        audioSource = gameObject.AddComponent<AudioSource>();

        // Display the "First to 3 Wins!" message
        firstTo3Text.text = "First to 3 Wins!";

        // Find and assign the GameUIController for Play Again functionality
        gameUIController = FindObjectOfType<GameUIController>();
    }

    void Update()
    {
        if (gameActive)
        {
            RunCountdown();
        }

        // Show player's gesture on the enemy gesture text UI
        playerGestureText.text = "Player Gesture: " + playerGesture;

        if (gameActive)
        {
            totalTime += Time.deltaTime;  // Keep track of the total game time
        }
    }

    public void StartNewRound()
    {
        resultText.text = "";
        countdown = countdownTime;
        enemyGesture = gestures[Random.Range(0, gestures.Length)];
        enemyGestureText.text = "Enemy Gesture: ???";
        gameActive = true;

        HideAllHandModels();

        //Reset all hand models to the inactive color
        ResetHandModelColors();
    }

    void RunCountdown()
    {
        int currentSecond = Mathf.CeilToInt(countdown); // Convert countdown to an integer for exact timing

        // Play countdown sound only once per second (at 3, 2, and 1)
        if (currentSecond > 0 && currentSecond != lastCountdownSecond)
        {
            audioSource.PlayOneShot(countdownClip); // Play countdown sound
            lastCountdownSecond = currentSecond; // Update the last played second
        }

        countdown -= Time.deltaTime;
        countdownText.text = "Time: " + Mathf.Ceil(countdown).ToString();

        if (countdown <= 0)
        {
            countdown = 0;
            gameActive = false;
            lastCountdownSecond = -1; // Reset for the next round
            ShowResult();
        }
    }

    void ShowResult()
    {
        Debug.Log("ShowResult called.");
        enemyGestureText.text = "Enemy Gesture: " + enemyGesture;

        // Highlight the corresponding hand model for the computer gesture
        HighlightComputerGesture();

        if ((playerGesture == "RockPoseLeft" && enemyGesture == "Rock") ||
            (playerGesture == "RockPoseRight" && enemyGesture == "Rock") ||
            (playerGesture == "PaperLeft" && enemyGesture == "Paper") ||
            (playerGesture == "PaperRight" && enemyGesture == "Paper") ||
            (playerGesture == "ScissorsLeft" && enemyGesture == "Scissors") ||
            (playerGesture == "ScissorsRight" && enemyGesture == "Scissors"))
        {
            resultText.text = "Draw!";
        }
        else if ((playerGesture == "RockPoseLeft" && enemyGesture == "Scissors") ||
                 (playerGesture == "RockPoseRight" && enemyGesture == "Scissors") ||
                 (playerGesture == "PaperLeft" && enemyGesture == "Rock") ||
                 (playerGesture == "PaperRight" && enemyGesture == "Rock") ||
                 (playerGesture == "ScissorsLeft" && enemyGesture == "Paper") ||
                 (playerGesture == "ScissorsRight" && enemyGesture == "Paper"))
        {
            resultText.text = "You Win!";
            playerScore++;
            audioSource.PlayOneShot(winClip); // Play win sound
        }
        else
        {
            resultText.text = "You Lose!";
            enemyScore++;
            audioSource.PlayOneShot(loseClip); // Play win soun
        }

        UpdateScoreUI(); // Update the score display


        // Check if either player or enemy has reached 3 points
        if (playerScore >= 3 || enemyScore >= 3)
        {
            DeclareWinner();
        }
        else
        {
            Invoke("StartNewRound", 3f);
        }
    }

    void DeclareWinner()
    {
        if (playerScore >= 3)
        {
            resultText.text = "Congratulations! You won the game!";
        }
        else if (enemyScore >= 3)
        {
            resultText.text = "Game Over! The enemy won the game!";
        }

        // Reset the scores after declaring a winner
        playerScore = 0;
        enemyScore = 0;
        UpdateScoreUI();

        // Show Play Again button through the GameUIController
        gameUIController.ShowPlayAgain();

        // Save the total game time to a file when the game ends
        SaveTimeToFile();
    }

    // Save the total time to a .txt file
    private void SaveTimeToFile()
    {
        string filePath = "Assets/TotalTime.txt";  // Path to save the file
        string timeToSave = "Total Time: " + totalTime.ToString("F2") + " seconds";  // Format the time

        // Append the time to the file (or create the file if it doesn't exist)
        File.AppendAllText(filePath, timeToSave + "\n");

        Debug.Log("Time saved to file: " + timeToSave);
    }

    // This method is called when the gesture is detected
    void OnGestureDetected(string gesture)
    {
        playerGesture = gesture; // Update the player's gesture
        Debug.Log("Player Gesture: " + playerGesture); // Debug: Log player gesture
    }

    // Highlight the hand model based on the computer's gesture
    void HighlightComputerGesture()
    {
        // Hide all hand models first
        HideAllHandModels();

        if (enemyGesture == "Rock")
        {
            Rock.SetActive(true);
            Rock.GetComponent<Renderer>().material = activeMaterial; // Change color to active
        }
        else if (enemyGesture == "Paper")
        {
            Paper.SetActive(true);
            Paper.GetComponent<Renderer>().material = activeMaterial; // Change color to active
        }
        else if (enemyGesture == "Scissors")
        {
            Scissors.SetActive(true);
            Scissors.GetComponent<Renderer>().material = activeMaterial; // Change color to active
        }
    }

    // Hide all hand models
    void HideAllHandModels()
    {
        Rock.SetActive(false);
        Paper.SetActive(false);
        Scissors.SetActive(false);
    }

    // Reset the colors of all hand models to inactive
    void ResetHandModelColors()
    {
        Rock.GetComponent<Renderer>().material = inactiveMaterial;
        Paper.GetComponent<Renderer>().material = inactiveMaterial;
        Scissors.GetComponent<Renderer>().material = inactiveMaterial;
    }

    // Update the score UI for both player and enemy
    void UpdateScoreUI()
    {
        playerScoreText.text = "Player Score: " + playerScore;
        enemyScoreText.text = "Enemy Score: " + enemyScore;
    }

    // Method to reset the game when Play Again is clicked
    public void ResetGame()
    {
        playerScore = 0;
        enemyScore = 0;
        UpdateScoreUI();
        totalTime = 0f;
        StartNewRound();
    }

    public bool IsGameActive()
    {
        return gameActive; // Returns true if the game is currently active
    }
}



