using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Handles Scene Transition
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    #region Singleton
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    // Loads Scene Asyncronously
    public IEnumerator LoadScene(string name)
    {
        AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync(name);

        // Activate Loading Panel
        PanelManager.Instance.ActivatePanel("loading-panel");

        while (!asyncLoadScene.isDone)
        {
            // Loading Bar
            float loadProgress = Mathf.Clamp01(asyncLoadScene.progress / .9f);

            Debug.Log("Loading Progress: " + loadProgress);

            yield return null;
        }

        Debug.Log("Loading Complete");
    }
}
