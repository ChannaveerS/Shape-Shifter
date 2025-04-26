using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class LevelTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 120f;
    private float timeRemaining;
    private bool timerRunning = true;

    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text messageText;
    public List<GameObject> playerObjects;

    private bool levelEnded = false;

    [Header("Message Animation")]
    public float slideDuration = 2f;
    public Vector3 startOffset = new Vector3(-800, 0, 0); // Left offscreen
    public Vector3 endOffset = new Vector3(800, 0, 0);    // Right offscreen

    private void Start()
    {
        timeRemaining = totalTime;
        UpdateTimerDisplay();

        if (messageText != null)
        {
            messageText.text = "";
        }
    }

    private void Update()
    {
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerRunning = false;
                if (!levelEnded)
                {
                    LevelFailed();
                }
            }

            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        int seconds = Mathf.CeilToInt(timeRemaining);

        if (timerText != null)
        {
            timerText.text = "Timer: " + seconds.ToString();
        }
    }

    public void LevelFailed()
    {
        Debug.Log("🛑 Time's up! Level failed.");
        levelEnded = true;
        FreezeAllPlayers();

        if (messageText != null)
        {
            messageText.text = "Level Failed, Restarting!!";
            messageText.color = new Color32(255, 92, 92, 255); // Coral Red
            StartCoroutine(SlideText());
        }

        StartCoroutine(RestartLevelAfterDelay(2f));
    }

    public void LevelCompleted()
    {
        Debug.Log("✅ Level completed successfully!");
        levelEnded = true;
        FreezeAllPlayers();

        if (messageText != null)
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;

            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                messageText.text = $"Cool! On to Level {nextIndex}!";
                messageText.color = new Color32(0, 255, 200, 255); // Mint Cyan
            }
            else
            {
                messageText.text = "Game Completed!";
                messageText.color = new Color32(255, 215, 0, 255); // Gold Yellow
            }

            StartCoroutine(SlideText());
        }

        StartCoroutine(LoadNextLevelAfterDelay(1f));
    }


    private void FreezeAllPlayers()
    {
        foreach (GameObject player in playerObjects)
        {
            if (player != null)
            {
                var movement = player.GetComponent<ShapeController>();
                if (movement != null)
                    movement.enabled = false;

                var rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                }
            }
        }
    }

    private IEnumerator RestartLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("🎉 No more levels. Game complete!");

            // Wait extra before returning to Main Menu
            yield return new WaitForSeconds(2f); // <-- ADD extra 2 seconds delay

            SceneManager.LoadScene(0); // Load Main Menu (Scene 0)
        }
    }



    private IEnumerator SlideText()
    {
        RectTransform rectTransform = messageText.GetComponent<RectTransform>();

        rectTransform.anchoredPosition = startOffset;
        Vector3 finalPosition = endOffset;

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(startOffset, finalPosition, elapsed / slideDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = finalPosition;
    }
}
