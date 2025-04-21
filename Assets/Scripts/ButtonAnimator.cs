using UnityEngine;
using System.Collections;

public class ButtonAnimator : MonoBehaviour
{
    public Vector3 pressedOffset = new Vector3(0f, -0.2f, 0f);
    public float moveDuration = 0.15f;

    public AudioClip pressSound;
    public AudioClip releaseSound;

    private Vector3 originalPosition;
    private Coroutine moveRoutine;
    private AudioSource audioSource;

    void Start()
    {
        originalPosition = transform.localPosition;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("playerTriangle") || other.CompareTag("playerCube"))
        {
            if (moveRoutine != null) StopCoroutine(moveRoutine);
            moveRoutine = StartCoroutine(MoveButton(originalPosition + pressedOffset));

            if (audioSource && pressSound)
                audioSource.PlayOneShot(pressSound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("playerTriangle") || other.CompareTag("playerCube"))
        {
            if (moveRoutine != null) StopCoroutine(moveRoutine);
            moveRoutine = StartCoroutine(MoveButton(originalPosition));

            if (audioSource && releaseSound)
                audioSource.PlayOneShot(releaseSound);
        }
    }

    private IEnumerator MoveButton(Vector3 targetPos)
    {
        float t = 0f;
        Vector3 startPos = transform.localPosition;

        while (t < moveDuration)
        {
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t / moveDuration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPos;
    }
}
