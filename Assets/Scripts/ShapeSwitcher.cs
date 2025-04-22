using UnityEngine;

public class ShapeSwitcher : MonoBehaviour
{
    public GameObject triangle1;
    public GameObject triangle2;

    private GameObject activePlayer;

    private void Start()
    {
        SetActivePlayer(triangle1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (activePlayer == triangle1)
                SetActivePlayer(triangle2);
            else
                SetActivePlayer(triangle1);
        }
    }

    private void SetActivePlayer(GameObject newPlayer)
    {
        activePlayer = newPlayer;

        // Enable movement only for the active triangle
        triangle1.GetComponent<ShapeController>().enabled = (newPlayer == triangle1);
        triangle2.GetComponent<ShapeController>().enabled = (newPlayer == triangle2);

        // Update glow emission color
        var mat1 = triangle1.GetComponent<Renderer>().material;
        var mat2 = triangle2.GetComponent<Renderer>().material;

        mat1.EnableKeyword("_EMISSION");
        mat2.EnableKeyword("_EMISSION");

        if (newPlayer == triangle1)
        {
            mat1.SetColor("_EmissionColor", Color.white);
            mat2.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            mat1.SetColor("_EmissionColor", Color.black);
            mat2.SetColor("_EmissionColor", Color.white);
        }
    }
}
