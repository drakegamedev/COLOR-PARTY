using UnityEngine;

// Handles UI Navigation through Panel Management
public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance;

    [System.Serializable]
    public struct PanelData
    {
        public string Id;
        public GameObject PanelObject;
    }

    [Header("References")]
    [SerializeField] private PanelData[] panels;

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
    }
    #endregion

    // Activate Selected Panel
    public void ActivatePanel(string id)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].PanelObject.SetActive(panels[i].Id == id);
        }
    }
}