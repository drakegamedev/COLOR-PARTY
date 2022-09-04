using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    // Game State
    public enum GameStates
    {
        INITIAL,
        PLAYING,
        GAME_OVER
    };
    
    public static GameManager Instance;

    public GameStates GameState { get; set; }

    public GameObject TimePanel;
    public TextMeshProUGUI CountdownText;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI GameResult;
    public TextMeshProUGUI PlayerStanding;

    [Header("Color References")]
    public Color[] LightColors;
    public Color[] DarkColors;
    public GameObject[] LightLerpWalls { get; private set; }
    public GameObject[] DarkLerpWalls { get; private set; }

    [Header("Aesthetic References")]
    public float LerpTime;
    public ParticleSystem ArenaAesthetic;                                               // Arena Particle System (Aesthetic)

    public List<GameObject> PlayerGameObjects { get; set; } = new();

    [Header("Player References")]
    public GameObject[] PlayerPrefabs;

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
            PhotonNetwork.Instantiate(PlayerPrefabs[number].name, PlayerSpawnManager.Instance.SpawnPoints[number].position, Quaternion.identity);
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

    // Initialize Wall Properties
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
            lerpColor.LerpTime = LerpTime;

            // Add to List
            lerpColorList.Add(lerpColor);
        }

        // Initialize Dark Walls
        foreach (GameObject go in DarkLerpWalls)
        {
            LerpColor lerpColor = go.AddComponent<LerpColor>();
            lerpColor.MyColors = DarkColors;
            lerpColor.LerpTime = LerpTime;

            // Add to List
            lerpColorList.Add(lerpColor);
        }
    }

    // Intensifies Game Atmosphere
    public void IntensifyAtmosphere()
    {
        // Faster Movement Speed of Arena Aesthetic Particles
        var main = ArenaAesthetic.main;
        main.simulationSpeed = 4f;
    }

    // Leave the Room
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    // When player leaves room
    // Redirect player back to Lobby Scene
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
