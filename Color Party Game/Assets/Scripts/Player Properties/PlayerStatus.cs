using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerStatus : MonoBehaviourPunCallbacks
{
    public int Place { get; set; }                              // Indicate Player Rank (e.g. 1,2,3, etc.)
    public string OrdinalPlace { get; private set; }            // Add Suffix to Player Rank (e.g. 1st, 2nd, 3rd)

    public override void OnDisable()
    {
        EventManager.Instance.EndGame -= AddToScoreList;
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.EndGame += AddToScoreList;
    }

    /*
    // Update is called once per frame
    void Update()
    {
        
    }*/

    #region StatusEffects
    #endregion

    #region PlayerStanding
    // Converts Integer to Ordinal Number
    public void Ordinalize(int number)
    {
        // Default suffix
        string numberSuffix = "TH";

        // Get last 2 digits
        int lastTwoDigits = number % 100;

        // Set Suffix as TH, if last 2 digits yields 11, 12, or 13
        if (lastTwoDigits < 11 || lastTwoDigits > 13)
        {
            // Check last digit of the number
            switch (lastTwoDigits % 10)
            {
                case 1:
                    // 1st Suffix
                    numberSuffix = "ST";
                    break;

                case 2:
                    // 2nd Suffix
                    numberSuffix = "ND";
                    break;

                case 3:
                    // 3rd Suffix
                    numberSuffix = "RD";
                    break;
            }
        }

        // Set Ordinal Place
        OrdinalPlace = number + numberSuffix;
    }

    // Shows Ranking of the Player
    public void ShowPlayerRank()
    {
        TextMeshProUGUI playerStandingText = GameManager.Instance.PlayerStanding;

        if (photonView.IsMine)
        {
            playerStandingText.text = "YOU REACHED " + OrdinalPlace + " PLACE.";
        }
    }

    // Add Player To Score List
    public void AddToScoreList()
    {
        ScoreManager.Instance.Players.Add(gameObject);
    }
    #endregion
}
