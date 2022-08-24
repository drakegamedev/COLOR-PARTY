using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Linq;

public class Despawner : PunRaiseEvents
{
    public string ObjectName { get; set; }             // Object Name
    public string ObjectID { get; set; }               // Object ID

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
            case (byte)RaiseEvents.DESPAWN_POWER_UP:
                object[] data = (object[])photonEvent.CustomData;

                string objectName = (string)data[0];
                string objectId = (string)data[1];

                foreach (GameObject go in ObjectPooler.Instance.PoolDictionary[objectId])
                {
                    // Disable object with designated ID
                    if (objectName == go.name)
                    {
                        go.GetComponent<Poolable>().ReturnToPool();
                        break;
                    }
                }
                break;
        }
    }

    public override void SetRaiseEvent()
    {
        // event data
        object[] data = new object[] { ObjectName, ObjectID };

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

        PhotonNetwork.RaiseEvent((byte)RaiseEvents.DESPAWN_POWER_UP, data, raiseEventOptions, sendOption);
    }
}
