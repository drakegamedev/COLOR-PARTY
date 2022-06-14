using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Tutorial Panels")]
    public GameObject[] tutorialPages;

    [Header("Credits Panels")]
    public GameObject[] creditsPages;

    private int tutorialPageNumber;
    private int creditsPageNumber;

    private void Start()
    {
        PanelManager.Instance.ActivatePanel("Main Menu");
        AudioManager.Instance.Play("MainMenuMusic");
        tutorialPageNumber = 0;
        creditsPageNumber = 0;
    }

    public void OnPlayButtonClicked()
    {
        StartCoroutine(AsyncLoadScene("LobbyScene"));
    }

    public void OnTutorialButtonClicked()
    {
        PanelManager.Instance.ActivatePanel("Tutorial");
    }

    public void OnTutorialReturnButtonClicked()
    {
        PanelManager.Instance.ActivatePanel("Main Menu");
        TutorialPanel(tutorialPages[0]);
    }

    public void OnCreditsButtonClicked()
    {
        PanelManager.Instance.ActivatePanel("Credits");
    }

    public void OnCreditsReturnButtonClicked()
    {
        PanelManager.Instance.ActivatePanel("Main Menu");
        CreditsPanel(creditsPages[0]);
    }

    public void OnQuitGameButtonClicked()
    {
        Debug.Log("You Quit the Game!");
        Application.Quit();
    }

    public void TutorialPanel(GameObject chosenPanel)
    {
        tutorialPages[0].SetActive(chosenPanel.Equals(tutorialPages[0]));
        tutorialPages[1].SetActive(chosenPanel.Equals(tutorialPages[1]));
    }

    public void CreditsPanel(GameObject chosenPanel)
    {
        creditsPages[0].SetActive(chosenPanel.Equals(creditsPages[0]));
        creditsPages[1].SetActive(chosenPanel.Equals(creditsPages[1]));
    }

    // Tutorial
    public void TutorialPrevious()
    {
        tutorialPageNumber--;

        if (tutorialPageNumber < 0)
        {
            tutorialPageNumber = 0;
        }

        TutorialPanel(tutorialPages[tutorialPageNumber]);
    }

    public void TutorialNext()
    {
        tutorialPageNumber++;

        if (tutorialPageNumber >= tutorialPages.Length)
        {
            tutorialPageNumber = tutorialPages.Length - 1;
        }

        TutorialPanel(tutorialPages[tutorialPageNumber]);
    }

    // Credits
    public void CreditsPrevious()
    {
        creditsPageNumber--;

        if (creditsPageNumber < 0)
        {
            creditsPageNumber = 0;
        }

        CreditsPanel(creditsPages[creditsPageNumber]);
    }

    public void CreditsNext()
    {
        creditsPageNumber++;

        if (creditsPageNumber >= creditsPages.Length)
        {
            creditsPageNumber = creditsPages.Length - 1;
        }

        CreditsPanel(creditsPages[creditsPageNumber]);
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
