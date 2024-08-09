using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Import for UI components

public class AnimationManager : MonoBehaviour
{
    //* Tutorial game object
    public GameObject tutorialPanel;
    public GameObject tutorialPlayButton;
    public GameObject tutorialStamp;

    //* UI Ingame game object
    public GameObject uiPanel;

    private CanvasGroup playButtonCanvasGroup;
    private CanvasGroup panelCanvasGroup;

    void Start()
    {
        // Get the CanvasGroup component from tutorialPlayButton
        playButtonCanvasGroup = tutorialPlayButton.GetComponent<CanvasGroup>();
        if (playButtonCanvasGroup == null)
        {
            // Add CanvasGroup if it doesn't already exist
            playButtonCanvasGroup = tutorialPlayButton.AddComponent<CanvasGroup>();
        }

        // Get the CanvasGroup component from tutorialPanel
        panelCanvasGroup = tutorialPanel.GetComponent<CanvasGroup>();
        if (panelCanvasGroup == null)
        {
            // Add CanvasGroup if it doesn't already exist
            panelCanvasGroup = tutorialPanel.AddComponent<CanvasGroup>();
        }

        // Start the repeating animation
        StartCoroutine(LoopFadeCanvasGroupOpacity(playButtonCanvasGroup, 1f, 0.2f));
    }

    public void OnTutorialPlayButtonPressed()
    {
        // Start the sequence of animations
        StartCoroutine(PlayButtonPressSequence());
    }

    private IEnumerator PlayButtonPressSequence()
    {
        tutorialStamp.SetActive(true);

        // Scale tutorialStamp from 2 to 1.8
        yield return StartCoroutine(ScaleTutorialStampAnimation(1f, 2f, 1.4f));

        // Fade tutorialPanel from 100 to 0
        yield return StartCoroutine(FadeCanvasGroupOpacity(panelCanvasGroup, 1f, 0f));

        // Set tutorialPanel to inactive
        tutorialPanel.SetActive(false);
    }

    private IEnumerator ScaleTutorialStampAnimation(float duration, float startScale, float endScale)
    {
        Vector3 startScaleVector = new Vector3(startScale, startScale, startScale);
        Vector3 endScaleVector = new Vector3(endScale, endScale, endScale);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float scale = Mathf.Lerp(startScale, endScale, elapsed / duration);
            tutorialStamp.transform.localScale = new Vector3(scale, scale, scale);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is set
        tutorialStamp.transform.localScale = endScaleVector;
    }

    private IEnumerator LoopFadeCanvasGroupOpacity(CanvasGroup canvasGroup, float duration, float targetAlpha)
    {
        while (true) // Infinite loop for continuous animation
        {
            // Fade out to targetAlpha
            yield return StartCoroutine(FadeCanvasGroupOpacity(canvasGroup, duration, targetAlpha));
            // Fade in back to 1 (full opacity)
            yield return StartCoroutine(FadeCanvasGroupOpacity(canvasGroup, duration, 1f));
        }
    }

    private IEnumerator FadeCanvasGroupOpacity(CanvasGroup canvasGroup, float duration, float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            canvasGroup.alpha = alpha;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha is set
        canvasGroup.alpha = targetAlpha;
    }
}
