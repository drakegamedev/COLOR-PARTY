using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

// Indicates Whether this Player is the Winner or the Loser
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

                // This is You
                if (viewId == photonView.ViewID)
                {
                    playerResultText.text = "YOU WIN!";
                }
                // This is for the Other players
                else
                {
                    playerResultText.text = "YOU LOSE!";
                }

                break;
        }
    }

    public override void SetRaiseEvent()
    {
        // Player View ID
        int viewId = photonView.ViewID;

        // Event Data
        object[] data = new object[] { viewId };

        // Assign Receivers
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        // Reliability
        SendOptions sendOption = new SendOptions
        {
            Reliability = false
        };

        PhotonNetwork.RaiseEvent((byte)RaiseEvents.PLAYER_WINNER, data, raiseEventOptions, sendOption);
    }
}
