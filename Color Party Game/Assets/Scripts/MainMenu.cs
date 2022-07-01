using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private int tutorialPageNumber;
    private int creditsPageNumber;

    // Start is called before the first frame update
    void Start()
    {
        PanelManager.Instance.ActivatePanel("MainMenuPanel");
        AudioManager.Instance.Play("MainMenuMusic");
        tutorialPageNumber = 0;
        creditsPageNumber = 0;
    }

    public void OnPlayButtonClicked()
    {
        Debug.Log("Play");
        StartCoroutine(AsyncLoadScene("LobbyScene"));
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
