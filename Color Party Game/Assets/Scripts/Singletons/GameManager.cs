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
    
    public GameObject[] PlayerPrefabs;

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
}
