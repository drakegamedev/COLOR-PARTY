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

    public TextMeshProUGUI[] PlayerRankTexts;
    public List<GameObject> Players { get; private set; } = new();
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
        for (int lastSortedIndex = Players.Count - 1; lastSortedIndex > 0; lastSortedIndex--)
        {
            for (int i = 0; i < lastSortedIndex; i++)
            {
                int playerScore1 = Players[i].GetComponent<PlayerScoring>().PlayerScore;
                int playerScore2 = Players[i + 1].GetComponent<PlayerScoring>().PlayerScore;

                if (playerScore1 < playerScore2)
                {
                    GameObject temp = Players[i];
                    Players[i] = Players[i + 1];
                    Players[i + 1] = temp;
                }
            }
        }

        PresentResults();
    }

     // Present the Results of the Game
    void PresentResults()
    {
        int order = 0;
        int place = 1;

        foreach (GameObject go in Players)
        {
            PlayerSetup playerSetup = go.GetComponent<PlayerSetup>();

            // RGB Floats
            float red = playerSetup.PlayerColor.r;
            float green = playerSetup.PlayerColor.g;
            float blue = playerSetup.PlayerColor.b;

            string playerName = go.GetComponent<PhotonView>().Owner.NickName;
            int playerScore = go.GetComponent<PlayerScoring>().PlayerScore;

            Debug.Log(playerName + " | " + playerScore);

            // Print place, name of player, then score
            PlayerRankTexts[order].text = "#" + place + " | " + playerName + " | Score: " + playerScore;

            // Modify text color to indicate player color
            PlayerRankTexts[order].color = new Color(red, green, blue);

            PlayerStatus playerStatus = go.GetComponent<PlayerStatus>();

            playerStatus.Place = place;
            playerStatus.Ordinalize(place);
            playerStatus.ShowPlayerRank();

            place++;
            order++;
        }

        Debug.Log("Winner: " + Players[0].GetComponent<PhotonView>().Owner.NickName);

        // Get Player RaiseEvent of the winner
        Players[0].GetComponent<WinLoseIndicator>().SetRaiseEvent();
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
