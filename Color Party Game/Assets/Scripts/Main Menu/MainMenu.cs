using UnityEngine;

// Contains All Main Menu Button Functions
public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PanelManager.Instance.ActivatePanel("main-menu-panel");
        AudioManager.Instance.Play("main-menu-bgm");
    }

    #region UI Button Functions
    /// <summary>
    /// Play Button
    /// </summary>
    public void OnPlayButtonClicked()
    {
        Debug.Log("Play");
        StartCoroutine(SceneLoader.Instance.LoadScene("LobbyScene"));
    }

    /// <summary>
    /// Tutorial Button
    /// </summary>
    public void OnTutorialButtonClicked()
    {
        PanelManager.Instance.ActivatePanel("tutorial-panel");
    }

    /// <summary>
    /// Credits Button
    /// </summary>
    public void OnCreditsButtonClicked()
    {
        PanelManager.Instance.ActivatePanel("credits-panel");
    }

    /// <summary>
    /// Quit Game Button
    /// </summary>
    public void OnQuitGameButtonClicked()
    {
        Application.Quit();
        Debug.Log("You have quit the game!");
    }
    #endregion
}
