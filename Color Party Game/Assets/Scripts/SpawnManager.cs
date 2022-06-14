using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    public static SpawnManager Instance;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public GameObject[] powerUps;

    public float time;
    private float currentTime;

    public enum RaiseEventsCode
    {
        SpawnPowerUpEventCode = 1
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
        if (photonEvent.Code == (byte)RaiseEventsCode.SpawnPowerUpEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;

            int randomIndex = (int)data[0];
            float randomX = (float)data[1];
            float randomY = (float)data[2];

            Debug.Log("Spawn Power-up");
            Instantiate(powerUps[randomIndex], new Vector2(randomX, randomY), Quaternion.identity);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTime = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerManager.Instance.timerActive == true)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                Spawner();
                currentTime = time;
            }
        }
    }

    public void Spawner()
    {
        int randomIndex = Random.Range(0, powerUps.Length);
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // event data
        object[] data = new object[] { randomIndex, randomX, randomY };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOption = new SendOptions
        {
            Reliability = false
        };

        PhotonNetwork.RaiseEvent((byte)RaiseEventsCode.SpawnPowerUpEventCode, data, raiseEventOptions, sendOption);
    }
}
