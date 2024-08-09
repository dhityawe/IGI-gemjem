using UnityEngine;

public class UIManager : MonoBehaviour
{
    //* Tutorial
    public GameObject tutorialPanel;
    public GameObject tutorialStamp;

    //* Pause
    public GameObject pausePanel;

    //* Game Over
    public GameObject gameOverPanel;

    void Start()
    {
        tutorialPanel.SetActive(true);
        Time.timeScale = 1;
    }
    public void PauseButton()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

}
