using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PanelManager.Instance.ActivatePanel("main-menu-panel");
        AudioManager.Instance.Play("main-menu-bgm");
    }

    public void OnPlayButtonClicked()
    {
        Debug.Log("Play");
        StartCoroutine(SceneLoader.Instance.LoadScene("LobbyScene"));
    }

    public void OnTutorialButtonClicked()
    {
        PanelManager.Instance.ActivatePanel("tutorial-panel");
    }

    public void OnCreditsButtonClicked()
    {
        PanelManager.Instance.ActivatePanel("credits-panel");
    }

    public void OnQuitGameButtonClicked()
    {
        Application.Quit();
        Debug.Log("You have quit the game!");
    }
}
