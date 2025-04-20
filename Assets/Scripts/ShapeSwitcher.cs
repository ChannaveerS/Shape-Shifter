using UnityEngine;

public class ShapeSwitcher : MonoBehaviour
{
    public GameObject playerTriangle;
    public GameObject playerCube;

    private GameObject activePlayer;

    private void Start()
    {
        SetActivePlayer(playerTriangle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (activePlayer == playerTriangle)
                SetActivePlayer(playerCube);
            else
                SetActivePlayer(playerTriangle);
        }
    }

    private void SetActivePlayer(GameObject newPlayer)
    {
        activePlayer = newPlayer;

        // Enable or disable movement
        playerTriangle.GetComponent<ShapeController>().enabled = (newPlayer == playerTriangle);
        playerCube.GetComponent<ShapeController>().enabled = (newPlayer == playerCube);

        // Set emission glow - white
        var triangleMat = playerTriangle.GetComponent<Renderer>().material;
        var cubeMat = playerCube.GetComponent<Renderer>().material;

        triangleMat.EnableKeyword("_EMISSION");
        cubeMat.EnableKeyword("_EMISSION");

        if (newPlayer == playerTriangle)
        {
            triangleMat.SetColor("_EmissionColor", Color.white);
            cubeMat.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            triangleMat.SetColor("_EmissionColor", Color.black);
            cubeMat.SetColor("_EmissionColor", Color.white);
        }
    }
}
