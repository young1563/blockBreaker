using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private int targetScore = 1000;
    [SerializeField] private float gameTime = 180f; // 3 minutes

    [Header("Current Game State")]
    private int currentScore = 0;
    private float remainingTime;
    private bool isGameActive = false;
    private int blocksRemaining = 0;

    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        GameOver,
        Victory
    }

    private GameState currentState = GameState.Menu;

    // Events
    public delegate void ScoreChanged(int newScore);
    public event ScoreChanged OnScoreChanged;

    public delegate void TimeChanged(float remainingTime);
    public event TimeChanged OnTimeChanged;

    public delegate void GameStateChanged(GameState newState);
    public event GameStateChanged OnGameStateChanged;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        remainingTime = gameTime;
    }

    void Update()
    {
        if (isGameActive && currentState == GameState.Playing)
        {
            // Update timer
            remainingTime -= Time.deltaTime;
            OnTimeChanged?.Invoke(remainingTime);

            // Check if time is up
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                GameOver(false);
            }

            // Check win condition
            if (currentScore >= targetScore)
            {
                GameOver(true);
            }
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        currentScore = 0;
        remainingTime = gameTime;
        currentState = GameState.Playing;
        
        OnScoreChanged?.Invoke(currentScore);
        OnTimeChanged?.Invoke(remainingTime);
        OnGameStateChanged?.Invoke(currentState);

        // Count blocks in scene
        CountBlocks();
    }

    public void PauseGame()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0f;
            OnGameStateChanged?.Invoke(currentState);
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = 1f;
            OnGameStateChanged?.Invoke(currentState);
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;
        OnScoreChanged?.Invoke(currentScore);
    }

    public void GameOver(bool victory)
    {
        isGameActive = false;
        currentState = victory ? GameState.Victory : GameState.GameOver;
        OnGameStateChanged?.Invoke(currentState);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void CountBlocks()
    {
        Block[] blocks = Object.FindObjectsByType<Block>(FindObjectsSortMode.None);
        blocksRemaining = blocks.Length;
    }

    // Getters
    public int GetCurrentScore() => currentScore;
    public float GetRemainingTime() => remainingTime;
    public GameState GetCurrentState() => currentState;
    public bool IsGameActive() => isGameActive;
}
