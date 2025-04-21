using UnityEngine;

public class ShapeController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float originalSpeed;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool exitingCutout = false;
    private AudioSource audioSource;

    public GameObject smokeEffectPrefab; // 💨 Assign explosion prefab here

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
        if (other.CompareTag("Cutout"))
        {
            Debug.Log("Entered cutout — slowing down");
            moveSpeed = originalSpeed * 0.4f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cutout"))
        {
            Debug.Log("Exited cutout — restoring speed, playing pop & smoke");
            moveSpeed = originalSpeed;

            if (audioSource != null && !audioSource.isPlaying)
                audioSource.Play();

            if (smokeEffectPrefab != null)
            {
                GameObject smoke = Instantiate(smokeEffectPrefab, transform.position, Quaternion.identity);
                Destroy(smoke, 2f);
            }

            StartCoroutine(PopOutEffect());
        }
    }

    private System.Collections.IEnumerator PopOutEffect()
    {
        exitingCutout = true;

        Vector3 popOffset = moveDirection.normalized * 2f;
        Vector3 targetPosition = rb.position + popOffset;

        float t = 0f;
        float duration = 0.15f;

        while (t < duration)
        {
            rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, t / duration));
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(targetPosition);
        exitingCutout = false;
    }
}
