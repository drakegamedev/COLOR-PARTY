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
    
    public GameObject[] PlayerGameObjects { get { return playerGameObjects; } set { playerGameObjects = value; } }
    public GameObject[] PlayerPrefabs;
    
    private GameObject[] playerGameObjects;

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
        if (PhotonNetwork.IsConnectedAndReady)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[i])
                {
                    number = i;
                    break;
                }
            }

            PhotonNetwork.Instantiate(PlayerPrefabs[number].name, PlayerSpawnManager.Instance.SpawnPoints[number].position, Quaternion.identity);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveRoom();
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
