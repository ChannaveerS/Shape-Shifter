using UnityEngine;

public class CutoutButtonTrigger : MonoBehaviour
{
    public CutoutRotator leftCutout;
    public CutoutRotator rightCutout;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("playerTriangle") || other.CompareTag("playerCube"))
        {
            leftCutout.Open();
            rightCutout.Open();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("playerTriangle") || other.CompareTag("playerCube"))
        {
            leftCutout.Close();
            rightCutout.Close();
        }
    }
}
