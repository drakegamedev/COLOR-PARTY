using UnityEngine;

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
    public PanelData[] Panels;

    #region Singleton
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public void ActivatePanel(string id)
    {
        for (int i = 0; i < Panels.Length; i++)
        {
            Panels[i].PanelObject.SetActive(Panels[i].Id == id);
        }
    }
}
