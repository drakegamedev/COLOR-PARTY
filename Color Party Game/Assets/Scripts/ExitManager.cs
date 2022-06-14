using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ExitManager : MonoBehaviourPunCallbacks
{
    public enum RaiseEventsCode
    {
        StartEventCode = 2
    }

    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventsCode.StartEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;

            StartCoroutine(GameManager.Instance.CountdownToStart());
        }
    }

    public void EventStart()
    {
        // event data
        object[] data = new object[] { };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOption = new SendOptions
        {
            Reliability = false
        };

        PhotonNetwork.RaiseEvent((byte)RaiseEventsCode.StartEventCode, data, raiseEventOptions, sendOption);
    }
}
