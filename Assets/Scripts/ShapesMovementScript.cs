using UnityEngine;

public class ShapeController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float originalSpeed;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool exitingCutout = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        originalSpeed = moveSpeed;
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
        if (other.gameObject.name == "Cutout")
        {
            Debug.Log("Entering cutout — slowing down");
            moveSpeed = originalSpeed * 0.3f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Cutout")
        {
            Debug.Log("Exiting cutout — restoring speed and popping out");
            moveSpeed = originalSpeed;
            StartCoroutine(PopOutEffect());
        }
    }

    // 🌀 Add a small forward push when exiting
    private System.Collections.IEnumerator PopOutEffect()
    {
        exitingCutout = true;

        // Displace forward slightly (based on current move direction)
        Vector3 popOffset = moveDirection.normalized * 2.5f; // tweak amount here
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
