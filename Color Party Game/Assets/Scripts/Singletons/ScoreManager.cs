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

    public TextMeshProUGUI[] playerRankText;
    
    // List of players to be sorted once game is finished
    public List<GameObject> players;

    private void Awake()
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

    // Sort Score in Descending Order
    public void SortScore()
    {
        for (int lastSortedIndex = players.Count - 1; lastSortedIndex > 0; lastSortedIndex--)
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

        PresentResults();
    }

    // Present the Results of the Game
    private void PresentResults()
    {
        int order = 0;
        int place = 1;

        foreach (GameObject go in players)
        {
            // RGB Floats
            float red = go.GetComponent<PlayerStatus>().r;
            float green = go.GetComponent<PlayerStatus>().g;
            float blue = go.GetComponent<PlayerStatus>().b;

            Debug.Log(go.GetComponent<PlayerStatus>().playerName + " | " + go.GetComponent<PlayerStatus>().playerScore);

            // Print place, name of player, then score
            playerRankText[order].text = "#" + place + " | " + go.GetComponent<PlayerStatus>().playerName + " | Score: " + go.GetComponent<PlayerStatus>().playerScore;

            // Modify text color to indicate player color
            playerRankText[order].color = new Color(red, green, blue);

            go.GetComponent<PlayerStatus>().SetPlace(place);
            go.GetComponent<PlayerStatus>().ToOrdinal(place);
            go.GetComponent<PlayerStatus>().ShowPlayerRank();

            place++;
            order++;
        }

        Debug.Log("Winner: " + players[0].GetComponent<PlayerStatus>().playerName);

        // Get Player RaiseEvent of the winner
        players[0].GetComponent<PlayerEvents>().PlayerStanding();
    }
}
