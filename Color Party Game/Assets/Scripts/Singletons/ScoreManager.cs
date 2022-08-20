using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    public static ScoreManager Instance;

    public Transform[] Containers;
    public GameObject PlayerScoreItem;
    public List<GameObject> ScoreItems { get; private set; } = new();
    public GameObject ScorePanel;

    private Dictionary<Photon.Realtime.Player, GameObject> scoreBoardItems = new();
    private int playerIndex;

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

    // Start is called before the first frame update
    void Start()
    {
        playerIndex = 0;
        
        // Give ScoreBoard Items to each player in the Photon Player List
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            AddScoreBoardItem(player);
        }

        /*for (int i = 0; i < ScorePanel.transform.childCount; i++)
        {
            ScoreItems.Add(ScorePanel.transform.GetChild(i).gameObject);
        }*/
    }

    // Adds the player to the scoreboard
    void AddScoreBoardItem(Photon.Realtime.Player player)
    {
        GameObject item = Instantiate(PlayerScoreItem, Containers[playerIndex]);
        item.transform.localScale = Vector3.one;

        TextMeshProUGUI scoreText = item.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        scoreText.color = GameManager.Instance.LightColors[playerIndex];

        scoreBoardItems[player] = item;
        ScoreItems.Add(item);
        playerIndex++;
    }

    // Sort Score in Descending Order
    public void SortScore()
    {
        /*for (int lastSortedIndex = players.Count - 1; lastSortedIndex > 0; lastSortedIndex--)
        {
            for (int i = 0; i < lastSortedIndex; i++)
            {
                if (players[i].GetComponent<PlayerStatus>().playerScore < players[i + 1].GetComponent<PlayerStatus>().playerScore)
                {
                    GameObject temp = players[i];
                    players[i] = players[i + 1];
                    players[i + 1] = temp;
                }
            }
        }

        PresentResults();*/
    }

    // Add Score Board Item of the Player that entered the Room
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        AddScoreBoardItem(newPlayer);
    }

    // Remove Score Board Item of the Player that left the Room
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        RemoveScoreBoardItem(otherPlayer);
    }

    // Remove Scoreboard Item Function
    void RemoveScoreBoardItem(Photon.Realtime.Player player)
    {
        Destroy(scoreBoardItems[player]);
        scoreBoardItems.Remove(player);
    }
}
