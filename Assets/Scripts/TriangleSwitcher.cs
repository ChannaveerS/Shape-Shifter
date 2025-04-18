using UnityEngine;

public class TriangleSwitcher : MonoBehaviour
{
    public GameObject[] trianglePlayers;
    private int currentIndex = 0;

    void Start()
    {
        // Enable control only for the first one
        for (int i = 0; i < trianglePlayers.Length; i++)
        {
            SetControlEnabled(trianglePlayers[i], i == currentIndex);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Disable current
            SetControlEnabled(trianglePlayers[currentIndex], false);

            // Switch to the next
            currentIndex = (currentIndex + 1) % trianglePlayers.Length;

            // Enable new one
            SetControlEnabled(trianglePlayers[currentIndex], true);
        }
    }

    void SetControlEnabled(GameObject obj, bool isEnabled)
    {
        var controller = obj.GetComponent<ShapeController>();
        if (controller != null)
            controller.enabled = isEnabled;

        // Change color based on active/inactive
        var renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Color activeColor = new Color32(88, 211, 247, 255);   // Electric Lime
            Color inactiveColor = new Color32(217, 217, 217, 255); // Soft Gray


            renderer.material.color = isEnabled ? activeColor : inactiveColor;

            Material mat = renderer.material;

            if (isEnabled)
            {
                mat.SetColor("_BaseColor", activeColor); // for URP
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", activeColor * 2f); // multiplied for glow
            }
            else
            {
                mat.SetColor("_BaseColor", inactiveColor);
                mat.SetColor("_EmissionColor", Color.black);
            }
        }


    }

}
