using System.Collections.Generic;
using UnityEngine;

public class ShapeSwitcher : MonoBehaviour
{
    public List<GameObject> playerShapes; // Assign all shapes here in the Inspector
    private int currentIndex = 0;

    private void Start()
    {
        if (playerShapes.Count > 0)
        {
            SetActivePlayer(currentIndex);
        }
        else
        {
            Debug.LogWarning("No player shapes assigned in ShapeSwitcher!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerShapes.Count > 1)
        {
            currentIndex = (currentIndex + 1) % playerShapes.Count;
            SetActivePlayer(currentIndex);
        }
    }

    private void SetActivePlayer(int index)
    {
        for (int i = 0; i < playerShapes.Count; i++)
        {
            var shape = playerShapes[i];
            bool isActive = (i == index);

            // Enable/Disable controller script
            ShapeController controller = shape.GetComponent<ShapeController>();
            if (controller != null)
                controller.enabled = isActive;

            // Optional: Set glow
            var mat = shape.GetComponent<Renderer>().material;
            if (mat != null)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", isActive ? Color.white : Color.black);
            }
        }
    }
}
