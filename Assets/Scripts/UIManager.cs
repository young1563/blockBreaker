using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    [Header("Color Indicator")]
    [SerializeField] private Image currentColorIndicator;
    [SerializeField] private Color[] indicatorColors;

    void Start()
    {
        // Subscribe to GameManager events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += UpdateScore;
            GameManager.Instance.OnTimeChanged += UpdateTimer;
            GameManager.Instance.OnGameStateChanged += UpdateGameState;
        }

        // Hide all panels initially
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= UpdateScore;
            GameManager.Instance.OnTimeChanged -= UpdateTimer;
            GameManager.Instance.OnGameStateChanged -= UpdateGameState;
        }
    }

    void UpdateScore(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + newScore.ToString();
        }
    }

    void UpdateTimer(float remainingTime)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void UpdateGameState(GameManager.GameState newState)
    {
        // Hide all panels first
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

        // Show appropriate panel
        switch (newState)
        {
            case GameManager.GameState.Paused:
                if (pausePanel != null) pausePanel.SetActive(true);
                break;

            case GameManager.GameState.GameOver:
                if (gameOverPanel != null)
                {
                    gameOverPanel.SetActive(true);
                    if (finalScoreText != null && GameManager.Instance != null)
                    {
                        finalScoreText.text = "Final Score: " + GameManager.Instance.GetCurrentScore();
                    }
                }
                break;

            case GameManager.GameState.Victory:
                if (victoryPanel != null)
                {
                    victoryPanel.SetActive(true);
                    if (finalScoreText != null && GameManager.Instance != null)
                    {
                        finalScoreText.text = "Final Score: " + GameManager.Instance.GetCurrentScore();
                    }
                }
                break;
        }
    }

    public void UpdateColorIndicator(BlockColor color)
    {
        if (currentColorIndicator != null && indicatorColors != null && indicatorColors.Length > (int)color)
        {
            currentColorIndicator.color = indicatorColors[(int)color];
        }
    }

    // Button handlers
    public void OnPauseButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGame();
        }
    }

    public void OnResumeButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResumeGame();
        }
    }

    public void OnRestartButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }

    public void OnQuitButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.QuitGame();
        }
    }
}
