using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    // Game States
    public enum GameStates
    {
        INITIAL,
        PLAYING,
        GAME_OVER
    };
    
    public static GameManager Instance;

    public GameStates GameState { get; set; }

    [field : SerializeField] public GameObject TimePanel { get; private set; }
    [field : SerializeField] public TextMeshProUGUI CountdownText { get; private set; }
    [field : SerializeField] public TextMeshProUGUI TimerText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI GameResult { get; private set; }
    [field: SerializeField] public TextMeshProUGUI PlayerStanding { get; private set; }

    //[Header("Color References")]
    [field: SerializeField, Header("Color References")] public Color[] LightColors { get; private set; }
    [field: SerializeField] public Color[] DarkColors { get; private set; }
    public GameObject[] LightLerpWalls { get; private set; }
    public GameObject[] DarkLerpWalls { get; private set; }

    [Header("Aesthetic References")]
    [SerializeField] private float lerpTime;
    [SerializeField] private ParticleSystem arenaAesthetic;                             // Arena Particle System (Aesthetic)

    public List<GameObject> PlayerGameObjects { get; set; } = new();

    [Header("Player References")]
    [SerializeField] private GameObject[] playerPrefabs;

    // Private Variables
    private List<LerpColor> lerpColorList = new();                                      // Lerp Color List
    private int number;                                                                 // Player's number based on player list

    public override void OnDisable()
    {
        EventManager.Instance.Intensify -= IntensifyAtmosphere;
    }

    #region Singleton
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Execute Once Player is Ready and Connected to Servers
        if (PhotonNetwork.IsConnectedAndReady)
        {
            // Get Index Reference from Photon Player List
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[i])
                {
                    number = i;
                    break;
                }
            }

            // Spawn Player Prefab
            PhotonNetwork.Instantiate(playerPrefabs[number].name, PlayerSpawnManager.Instance.SpawnPoints[number].position, Quaternion.identity);
        }

        PanelManager.Instance.ActivatePanel("time-panel");
        EventManager.Instance.Intensify += IntensifyAtmosphere;

        InitializeWalls();
    }

    // Update is called once per frame
    void Update()
    {
        // Press Esc Key to leave room
        if (Input.GetKeyDown(KeyCode.Escape) && GameState == GameStates.PLAYING)
        {
            LeaveRoom();
        }
    }

    /// <summary>
    /// Initialize Wall Properties
    /// </summary>
    public void InitializeWalls()
    {
        // Cache In Walls
        LightLerpWalls = GameObject.FindGameObjectsWithTag("LightLerpColor");
        DarkLerpWalls = GameObject.FindGameObjectsWithTag("DarkLerpColor");

        // Initialize Light Walls
        foreach (GameObject go in LightLerpWalls)
        {
            LerpColor lerpColor = go.AddComponent<LerpColor>();
            lerpColor.MyColors = LightColors;
            lerpColor.LerpTime = lerpTime;

            // Add to List
            lerpColorList.Add(lerpColor);
        }

        // Initialize Dark Walls
        foreach (GameObject go in DarkLerpWalls)
        {
            LerpColor lerpColor = go.AddComponent<LerpColor>();
            lerpColor.MyColors = DarkColors;
            lerpColor.LerpTime = lerpTime;

            // Add to List
            lerpColorList.Add(lerpColor);
        }
    }

    /// <summary>
    /// Intensifies Game Atmosphere
    /// </summary>
    public void IntensifyAtmosphere()
    {
        // Faster Movement Speed of Arena Aesthetic Particles
        var main = arenaAesthetic.main;
        main.simulationSpeed = 4f;
    }

    /// <summary>
    /// Leave the Room
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// When player leaves room, Redirect player back to Lobby Scene
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
