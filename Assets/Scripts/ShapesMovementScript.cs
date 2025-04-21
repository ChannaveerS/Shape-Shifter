using UnityEngine;

public class ShapeController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float originalSpeed;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool exitingCutout = false;
    private AudioSource audioSource;
    public ParticleSystem smokeEffect; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        originalSpeed = moveSpeed;

        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ).normalized;
    }

    void FixedUpdate()
    {
        if (moveDirection != Vector3.zero && !exitingCutout)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "LeftCutout")
        {
            Debug.Log("Exiting cutout — restoring speed and popping out");
            moveSpeed = originalSpeed;

            if (audioSource != null)
            {
                audioSource.Play(); // 🔊 Play the pop sound
            }

            StartCoroutine(PopOutEffect());
        }
        if (other.gameObject.name == "RightCutout")
        {
            Debug.Log("Exiting cutout — restoring speed and popping out");
            moveSpeed = originalSpeed;

            if (audioSource != null)
            {
                audioSource.Play(); // 🔊 Play the pop sound
            }

            StartCoroutine(PopOutEffect());
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cutout"))
        {
            Debug.Log("Exiting cutout — restoring speed and popping out");
            moveSpeed = originalSpeed;

            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play(); // 🔊 Pop-out sound
            }

            if (smokeEffect != null)
            {
                smokeEffect.transform.position = transform.position;
                smokeEffect.Play(); // 💨 Trigger smoke puff
            }

            StartCoroutine(PopOutEffect());
        }
    }


    // 🌀 Add a small forward push when exiting
    private System.Collections.IEnumerator PopOutEffect()
    {
        exitingCutout = true;

        // Displace forward slightly (based on current move direction)
        Vector3 popOffset = moveDirection.normalized * 2f; // tweak amount here
        Vector3 targetPosition = rb.position + popOffset;

        float t = 0;
        float duration = 0.15f;

        while (t < duration)
        {
            rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, t / duration));
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(targetPosition); // Ensure exact position
        exitingCutout = false;
    }
}
