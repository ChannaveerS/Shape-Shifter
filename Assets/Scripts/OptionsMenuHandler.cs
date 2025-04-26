using UnityEngine;

public class OptionsMenuHandler : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;

    public void ShowOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void BackToMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}
