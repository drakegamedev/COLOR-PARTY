using System.Collections;
using System.Collections.Generic;
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
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
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
