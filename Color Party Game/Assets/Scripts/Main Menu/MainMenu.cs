using UnityEngine;

// Contains All Main Menu Button Functions
public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PanelManager.Instance.ActivatePanel("main-menu-panel");
        // AudioManager.Instance.Play("main-menu-bgm");
    }

    // Play Button
    public void OnPlayButtonClicked()
    {
        Debug.Log("Play");
        StartCoroutine(SceneLoader.Instance.LoadScene("LobbyScene"));
    }

    // Tutorial Button
    public void OnTutorialButtonClicked()
    {
        PanelManager.Instance.ActivatePanel("tutorial-panel");
    }

    // Credits Button
    public void OnCreditsButtonClicked()
    {
        PanelManager.Instance.ActivatePanel("credits-panel");
    }

    // Quit Game Button
    public void OnQuitGameButtonClicked()
    {
        Application.Quit();
        Debug.Log("You have quit the game!");
    }
}
