using UnityEngine;
using TMPro;

public class InstructionUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text moveInstructionText;
    public TMP_Text switchInstructionText;

    [Header("Display Settings")]
    public float displayDuration = 5f; // how long to show instructions

    private void Start()
    {
        if (moveInstructionText != null)
        {
            moveInstructionText.text = "Use Arrow Keys to Move";
            moveInstructionText.gameObject.SetActive(true);
        }

        if (switchInstructionText != null)
        {
            switchInstructionText.text = "Press SPACE to Switch Player";
            switchInstructionText.gameObject.SetActive(true);
        }

        // Hide after delay
        Invoke(nameof(HideInstructions), displayDuration);
    }

    private void HideInstructions()
    {
        if (moveInstructionText != null)
            moveInstructionText.gameObject.SetActive(false);

        if (switchInstructionText != null)
            switchInstructionText.gameObject.SetActive(false);
    }
}
