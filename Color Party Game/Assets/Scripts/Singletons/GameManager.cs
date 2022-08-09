using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [Header("Aesthetic References")]
    public Color[] LightColors;
    public Color[] DarkColors;
    public GameObject[] LightLerpWalls;
    public GameObject[] DarkLerpWalls;
    public float LerpTime;
    public ParticleSystem ArenaAesthetic;   // Arena Particle System (Aesthetic)

    public GameObject[] PlayerGameObjects { get { return playerGameObjects; } set { playerGameObjects = value; } }

    [Header("Player References")]
    public GameObject[] PlayerPrefabs;
    
    private GameObject[] playerGameObjects;
    private List<LerpColor> lerpColorList = new();

    // Player's number beased on player list
    private int number;

    #region Singleton
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Execute once player is ready and connected to servers
        if (PhotonNetwork.IsConnectedAndReady)
        {
            // Get index reference from Photon Player List
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
    }

    void Update()
    {
        // Press Esc Key to leave room
        if (Input.GetKeyDown(KeyCode.Escape))
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
            lerpColor.enabled = false;

            // Add to List
            lerpColorList.Add(lerpColor);
        }

        // Initialize Dark Walls
        foreach (GameObject go in DarkLerpWalls)
        {
            LerpColor lerpColor = go.AddComponent<LerpColor>();
            lerpColor.MyColors = DarkColors;
            lerpColor.LerpTime = LerpTime;
            lerpColor.enabled = false;

            // Add to List
            lerpColorList.Add(lerpColor);
        }
    }

    // Activates Player Setup
    public void SetUpPlayers()
    {
        for (int i = 0; i < playerGameObjects.Length; i++)
        {
            playerGameObjects[i].GetComponent<PlayerSetup>().SetPlayerViews();
        }
    }

    // Intensifies Game Atmosphere
    public void IntensifyAtmosphere()
    {
        // Faster Movement Speed of Arena Aesthetic Particles
        var main = ArenaAesthetic.main;
        main.simulationSpeed = 4f;

        // Enable Lerping of Colors
        // For the Walls
        foreach(LerpColor lcl in lerpColorList)
        {
            lcl.enabled = true;
        }
    }

    // Disables All Player Movements
    public void DisablePlayerMovements()
    {
        for (int i = 0; i < playerGameObjects.Length; i++)
        {
            playerGameObjects[i].GetComponent<PlayerMovement>().enabled = false;
        }
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
