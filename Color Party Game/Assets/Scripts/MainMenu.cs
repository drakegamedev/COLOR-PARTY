using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PanelManager.Instance.ActivatePanel("MainMenuPanel");
        AudioManager.Instance.Play("main-menu-bgm");
    }

    public void OnPlayButtonClicked()
    {
        Debug.Log("Play");
        StartCoroutine(AsyncLoadScene("LobbyScene"));
    }

    public void OnTutorialButtonClicked()
    {
        
    }

    public void OnCreditsButtonClicked()
    {

    }

    public void OnQuitGameButtonClicked()
    {

    }

    IEnumerator AsyncLoadScene(string name)
    {
        AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync(name);

        // Activate Loading Panel
        PanelManager.Instance.ActivatePanel("LoadingPanel");

        while (!asyncLoadScene.isDone)
        {
            // Loading bar
            float loadProgress = Mathf.Clamp01(asyncLoadScene.progress / .9f);

            Debug.Log("Loading Progress: " + loadProgress);

            yield return null;
        }

        Debug.Log("Loading Complete");
    }
}
