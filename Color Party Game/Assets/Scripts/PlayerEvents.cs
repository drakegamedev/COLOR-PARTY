using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerEvents : MonoBehaviourPunCallbacks
{
    public enum RaiseEventsCode
    {
        WhoWonEventCode = 0
    }

    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        Debug.Log("Enabled");
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        Debug.Log("Disabled");
    }

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventsCode.WhoWonEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;

            int viewId = (int)data[0];

            TextMeshProUGUI playerResultText = GameManager.Instance.playerResult;

            if (viewId == photonView.ViewID)    // This is you!
            {
                playerResultText.text = "YOU WIN!";
                Debug.Log("Win");
            }
            else
            {
                playerResultText.text = "YOU LOSE!";
                Debug.Log("Lose");
            }
        }
    }

    public void PlayerStanding()
    {
        Debug.Log(this.GetComponent<PlayerStatus>().playerName + " Event Has Been Called!");
        
        int viewId = photonView.ViewID;
        string place = GetComponent<PlayerStatus>().GetOrdinalPlace();

        // event data
        object[] data = new object[] { viewId};

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOption = new SendOptions
        {
            Reliability = false
        };

        PhotonNetwork.RaiseEvent((byte)RaiseEventsCode.WhoWonEventCode, data, raiseEventOptions, sendOption);
    }
}
