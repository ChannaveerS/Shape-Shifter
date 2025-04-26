using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LevelIntroText : MonoBehaviour
{
    public TextMeshProUGUI levelNameText;
    public float moveSpeed = 400f;
    public float displayDuration = 2f;
    private RectTransform rectTransform;
    private Vector2 startPos;
    private Vector2 endPos;

    private void Start()
    {
        rectTransform = levelNameText.GetComponent<RectTransform>();

        // Start position (off the left of screen)
        startPos = new Vector2(-Screen.width, rectTransform.anchoredPosition.y);

        // End position (off the right of screen)
        endPos = new Vector2(Screen.width, rectTransform.anchoredPosition.y);

        rectTransform.anchoredPosition = startPos;

        // Set text to current level
        string currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        levelNameText.text = currentLevel;
        levelNameText.text =  currentLevel;

        StartCoroutine(MoveText());
    }

    private IEnumerator MoveText()
    {
        float elapsed = 0f;
        float totalMoveTime = (endPos.x - startPos.x) / moveSpeed;

        while (elapsed < totalMoveTime)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsed / totalMoveTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = endPos;

        // Wait for displayDuration
        yield return new WaitForSeconds(displayDuration);

        Destroy(levelNameText.gameObject); // Remove after shown
    }
}
