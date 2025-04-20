using UnityEngine;

public class CutoutRotator : MonoBehaviour
{
    public enum CutoutSide { Left, Right }
    public CutoutSide side;

    public float rotationSpeed = 3f;

    private Quaternion originalRotation;
    private Quaternion targetRotation;
    private Quaternion openRotation;

    void Start()
    {
        originalRotation = transform.localRotation;

        // Use your correct target rotation based on side
        if (side == CutoutSide.Left)
        {
            // Goal: X = 0, Y = -90, Z = 90
            openRotation = Quaternion.Euler(0f, -90f, 90f);
        }
        else // Right
        {
            // Goal: X = 180, Y = -90, Z = -90
            openRotation = Quaternion.Euler(180f, -90f, -90f);
        }

        targetRotation = originalRotation; // stay idle initially
    }

    void Update()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void Open()
    {
        targetRotation = openRotation;
    }

    public void Close()
    {
        targetRotation = originalRotation;
    }
}
