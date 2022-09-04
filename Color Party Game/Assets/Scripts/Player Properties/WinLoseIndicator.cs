using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class WinLoseIndicator : PunRaiseEvents
{   
    public override void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public override void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public override void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case (byte)RaiseEvents.PLAYER_WINNER:

                object[] data = (object[])photonEvent.CustomData;
                int viewId = (int)data[0];
                TextMeshProUGUI playerResultText = GameManager.Instance.GameResult;

                // This is you!
                if (viewId == photonView.ViewID)
                {
                    playerResultText.text = "YOU WIN!";
                }
                // This is the other players
                else
                {
                    playerResultText.text = "YOU LOSE!";
                }

                break;
        }
    }

    public override void SetRaiseEvent()
    {
        Debug.Log(GetComponent<PhotonView>().Owner.NickName + " Event Has Been Called!");

        int viewId = photonView.ViewID;

        // event data
        object[] data = new object[] { viewId };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOption = new SendOptions
        {
            Reliability = false
        };

        PhotonNetwork.RaiseEvent((byte)RaiseEvents.PLAYER_WINNER, data, raiseEventOptions, sendOption);
    }
}
