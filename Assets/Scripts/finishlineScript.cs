using UnityEngine;
using System.Collections;

public class finishlineScript : MonoBehaviour
{
    private AudioSource audioSource;

    [Tooltip("Delay before freezing movement after crossing the finish line")]
    public float disableDelay = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("playerTriangle") || other.CompareTag("playerCube"))
        {
            Debug.Log("🎉 Finish Line Crossed by: " + other.gameObject.name);

            // Play chime sound
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }

            // Start coroutine to disable movement after short delay
            StartCoroutine(DisableMovementAfterDelay(other.gameObject, disableDelay));
        }
    }

    private IEnumerator DisableMovementAfterDelay(GameObject player, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Disable shape movement script
        var movementScript = player.GetComponent<ShapeController>();
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        // Freeze Rigidbody motion
        var rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        // Optional: Add animation trigger or particle effect
        // player.GetComponent<Animator>()?.SetTrigger("Celebrate");
    }
}
