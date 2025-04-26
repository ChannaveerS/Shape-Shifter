using UnityEngine;

public class OptionsInstructions : MonoBehaviour
{
    [Header("Instruction Panel")]
    public GameObject instructionPanel;

    public void ShowInstructions()
    {
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
        }
    }

    public void HideInstructions()
    {
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(false);
        }
    }
}
