using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

// Takes Charge of Spawning Random Power-Ups in Random Positions
public class PowerUpSpawner : PunRaiseEvents
{
    [Header("Power-Up Id's")]
    [SerializeField] private string[] powerUpNames;                           // Power-Up ID's

    [Header("Random Position Range")]
    [SerializeField] private float minX;                                      // Minimum X Position
    [SerializeField] private float maxX;                                      // Maximum X Position
    [SerializeField] private float minY;                                      // Minimum Y Position
    [SerializeField] private float maxY;                                      // Maximum Y Position

    [Header("Spawn Timer")]
    [SerializeField] private float timer;                                     // Timer for Spawning

    // Private Variables
    private float currentTime;                                                // Current Spwan Time

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

                // Spawn Power-Up from Pool
                ObjectPooler.Instance.SpawnFromPool(randomPowerUp, new Vector2(randomX, randomY), Quaternion.identity);

                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTime = timer;
    }

    // Update is called once per frame
    void Update()
    {
        // Initiate Timer once game has started,
        // then terminate once game is over
        if (GameManager.Instance.GameState != GameManager.GameStates.PLAYING)
            return;
        
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            SetRaiseEvent();

            // Reset Timer
            currentTime = timer;
        }
    }

    /// <summary>
    /// Syncronized Spawning of Power-Up
    /// </summary>
    public override void SetRaiseEvent()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        string randomPowerUp = powerUpNames[Random.Range(0, powerUpNames.Length)];

        // Event data
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
