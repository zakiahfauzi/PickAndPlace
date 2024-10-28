using UnityEngine;
using TMPro;
using Oculus.Interaction.Samples;

public class RockPaperScissors : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI enemyGestureText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI playerGestureText; 
    public TextMeshProUGUI playerScoreText; 
    public TextMeshProUGUI enemyScoreText;

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
        StartNewRound();

        // Set up the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (gameActive)
        {
            RunCountdown();
        }

        // Show player's gesture on the enemy gesture text UI
        playerGestureText.text = "Player Gesture: " + playerGesture;
    }

    void StartNewRound()
    {
        resultText.text = "";
        countdown = countdownTime;
        enemyGesture = gestures[Random.Range(0, gestures.Length)];
        enemyGestureText.text = "Enemy Gesture: ???";
        gameActive = true;

        //Reset all hand models to the inactive color
        ResetHandModelColors();
    }

    void RunCountdown()
    {
        if (Mathf.Ceil(countdown) != Mathf.Ceil(countdown - Time.deltaTime) && countdown > 0)
        {
            // Play countdown sound on each tick
            audioSource.PlayOneShot(countdownClip);
        }

        countdown -= Time.deltaTime;
        countdownText.text = "Time: " + Mathf.Ceil(countdown).ToString();

        if (countdown <= 0)
        {
            countdown = 0;
            gameActive = false;
            ShowResult();
        }
    }

    void ShowResult()
    {
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
            audioSource.PlayOneShot(winClip); // Play win soun
        }
        else
        {
            resultText.text = "You Lose!";
            enemyScore++;
            audioSource.PlayOneShot(loseClip); // Play win soun
        }

        UpdateScoreUI(); // Update the score display
        Invoke("StartNewRound", 3f); // Start a new round after a delay
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
        if (enemyGesture == "Rock")
        {
            Rock.GetComponent<Renderer>().material = activeMaterial; // Change color to active
        }
        else if (enemyGesture == "Paper")
        {
            Paper.GetComponent<Renderer>().material = activeMaterial; // Change color to active
        }
        else if (enemyGesture == "Scissors")
        {
            Scissors.GetComponent<Renderer>().material = activeMaterial; // Change color to active
        }
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
}



