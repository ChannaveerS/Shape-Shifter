using UnityEngine;
using System.Collections;

public class CutoutButtonRotator : MonoBehaviour
{
    [Tooltip("The cutout object to rotate")]
    public Transform targetCutout;

    [Tooltip("Rotation when button is pressed")]
    public Vector3 pressedRotation = new Vector3(90, 0, 0);

    [Tooltip("Rotation when button is released")]
    public Vector3 originalRotation = new Vector3(0, 90, 90);

    [Tooltip("Rotation speed (degrees per second)")]
    public float rotationSpeed = 180f;

    private Coroutine rotationCoroutine;

    private void Start()
    {
        if (targetCutout != null)
        {
            targetCutout.eulerAngles = originalRotation;
        }
        else
        {
            Debug.LogError("CutoutButtonRotator: No targetCutout assigned!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            Debug.Log("🔵 Player entered button, rotating to pressed");
            StartRotation(pressedRotation);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            Debug.Log("🔴 Player exited button, rotating back to original");
            StartRotation(originalRotation);
        }
    }

    private void StartRotation(Vector3 targetEulerAngles)
    {
        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        rotationCoroutine = StartCoroutine(RotateSmoothly(targetCutout, targetEulerAngles));
    }

    private IEnumerator RotateSmoothly(Transform obj, Vector3 targetEulerAngles)
    {
        Quaternion startRotation = obj.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);

        float angle = Quaternion.Angle(startRotation, targetRotation);
        float duration = angle / rotationSpeed; // How long to rotate based on speed

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            obj.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        obj.rotation = targetRotation; // Snap exactly at the end
    }

    private bool IsPlayer(Collider other)
    {
        return other.CompareTag("playerTriangle") || other.CompareTag("playerCube");
    }
}
