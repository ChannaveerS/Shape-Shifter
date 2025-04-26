using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class finishlineScript : MonoBehaviour
{
    private AudioSource audioSource;

    [Tooltip("Delay before transitioning after all players finish")]
    public float levelCompleteDelay = 1f;

    [Tooltip("List of all player objects that must finish")]
    public List<GameObject> playerObjects;

    private HashSet<GameObject> playersFinished = new HashSet<GameObject>();

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (playerObjects == null || playerObjects.Count == 0)
        {
            Debug.LogWarning("⚠️ No player objects assigned to finishlineScript.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playerObjects.Contains(other.gameObject)) return;

        if (!playersFinished.Contains(other.gameObject))
        {
            playersFinished.Add(other.gameObject);
            Debug.Log("✅ Player finished: " + other.gameObject.name);

            if (audioSource != null && !audioSource.isPlaying)
                audioSource.Play();

            // Glow
            if (other.TryGetComponent(out Renderer r))
            {
                StartCoroutine(PulseGlow(r, Color.cyan)); // 🎨 Customize color
            }

            if (playersFinished.Count == playerObjects.Count)
            {
                Debug.Log("🎉 All players finished! Proceeding to next level...");

                // Stop timer
                if (FindObjectOfType<LevelTimer>() is LevelTimer timer)
                {
                    timer.LevelCompleted();
                }

                // Message
                var ui = FindObjectOfType<UIManager>();
                if (ui != null)
                {
                    int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
                    ui.ShowLevelMessage($"Cool! On to Level {nextLevel}");
                }

                StartCoroutine(FreezeAndLoadNextLevel());
            }
        }
    }

    private IEnumerator FreezeAndLoadNextLevel()
    {
        yield return new WaitForSeconds(levelCompleteDelay);

        foreach (GameObject player in playerObjects)
        {
            if (player.TryGetComponent(out ShapeController movement))
                movement.enabled = false;

            if (player.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        yield return new WaitForSeconds(0.5f); // Optional pause

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("🏁 Game finished! No more levels.");
        }
    }

    private IEnumerator PulseGlow(Renderer renderer, Color pulseColor, float duration = 1.5f)
    {
        Material mat = renderer.material; // creates unique instance
        mat.EnableKeyword("_EMISSION");

        float t = 0f;
        while (t < duration)
        {
            float intensity = Mathf.PingPong(Time.time * 3f, 1f);
            mat.SetColor("_EmissionColor", pulseColor * intensity);
            t += Time.deltaTime;
            yield return null;
        }

        mat.SetColor("_EmissionColor", pulseColor * 1.2f);
    }
}
