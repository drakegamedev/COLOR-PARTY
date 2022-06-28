using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public List<GameObject> ScoreItems { get { return scoreItems; } set { scoreItems = value; } }
    public GameObject ScorePanel;
    
    private List<GameObject> scoreItems;

    #region Singleton
    void Awake()
    {
        // Singleton Pattern
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

    // Start is called before the first frame update
    void Start()
    {
        scoreItems = new();

        for (int i = 0; i < ScorePanel.transform.childCount; i++)
        {
            scoreItems.Add(ScorePanel.transform.GetChild(i).gameObject);
        }
    }
}
