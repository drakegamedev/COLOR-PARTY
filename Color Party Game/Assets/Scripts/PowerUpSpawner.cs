using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PowerUpSpawner : PunRaiseEvents
{
    [Header("Power-Up Id's")]
    public string[] PowerUpNames;

    [Header("Random Position Range")]
    public float MinX;
    public float MaxX;
    public float MinY;
    public float MaxY;

    [Header("Spawn Timer")]
    public float Timer;

    private float currentTime;

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
            case (byte)RaiseEvents.SPAWN_POWER_UP:
                object[] data = (object[])photonEvent.CustomData;

                float randomX = (float)data[0];
                float randomY = (float)data[1];
                string randomPowerUp = (string)data[2];

                ObjectPooler.Instance.SpawnFromPool(randomPowerUp, new Vector2(randomX, randomY), Quaternion.identity);
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTime = Timer;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0 && photonView.IsMine)
        {
            SetRaiseEvent();

            // Reset Timer
            currentTime = Timer;
        }
    }

    public override void SetRaiseEvent()
    {
        float randomX = Random.Range(MinX, MaxX);
        float randomY = Random.Range(MinY, MaxY);
        string randomPowerUp = PowerUpNames[Random.Range(0, PowerUpNames.Length)];

        // event data
        object[] data = new object[] { randomX, randomY, randomPowerUp };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOption = new SendOptions
        {
            Reliability = false
        };

        PhotonNetwork.RaiseEvent((byte)RaiseEvents.SPAWN_POWER_UP, data, raiseEventOptions, sendOption);
    }
}
