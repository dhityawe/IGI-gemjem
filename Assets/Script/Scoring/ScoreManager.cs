using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI[] scoreTexts;  // Array of TextMeshProUGUI for multiple score displays
    public TextMeshProUGUI highScoreText;

    public int currentScore;
    public int highScore;
    
    public bool isGameOver = false;

    // Reference to other scripts
    public UIManager uiManager;
    public SaveDataSO saveDataSO;

    void Start()
    {
        currentScore = 0;
    }

    void Awake()
    {
        // Load high score from saveDataSO
        highScore = saveDataSO.highScore;
        highScoreText.text = highScore.ToString();
    }

    void Update()
    {
        UpdateScoreTexts();

        if (isGameOver)
        {
            CalculateScore();
        }
    }

    void UpdateScoreTexts()
    {
        // Update all scoreTexts in the array
        foreach (var scoreText in scoreTexts)
        {
            scoreText.text = currentScore.ToString();
        }
    }

    void CalculateScore()
    {
        uiManager.gameOverPanel.SetActive(true);

        if (currentScore > highScore)
        {
            highScore = currentScore;
            highScoreText.text = highScore.ToString();
            saveDataSO.highScore = highScore;
        }
    }
}
