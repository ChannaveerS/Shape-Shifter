using UnityEngine;

public class finishlineScript : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("playerTriangle"))
        {
            Debug.Log("🎉 Finish Line Crossed!");

            // Play chime sound
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }

            // Start delayed disable coroutine
            StartCoroutine(DisableMovementAfterDelay(other.gameObject, 0.5f));
        }
    }

    private System.Collections.IEnumerator DisableMovementAfterDelay(GameObject player, float delay)
    {
        yield return new WaitForSeconds(delay);

        ShapeController movementScript = player.GetComponent<ShapeController>();
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
