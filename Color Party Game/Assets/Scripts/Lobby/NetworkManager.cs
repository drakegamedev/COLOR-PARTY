using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    [SerializeField] private TMP_InputField playerNameInput;                            // InputField for Player's Username
    [SerializeField] private KeyCode[] enterInputs;                                     // Enter Input Key Array

    [Header("Create Room Panel")]
    [SerializeField] private TMP_InputField roomNameInputField;                         // InputField for Room Name
    private int maxPlayers;                                                             // Maximum Player Amount

    [Header("Show Room List Panel")]
    [SerializeField] private GameObject roomListItemPrefab;                             // Room List Item Prefab GameObject Reference
    [SerializeField] private GameObject roomListParent;                                 // Room List Parent Reference

    [Header("Inside Room Panel")]
    [SerializeField] private TextMeshProUGUI roomNameText;                              // Room Name Text Reference
    [SerializeField] private TextMeshProUGUI playerCountText;                           // Player Count Text Reference
    [SerializeField] private GameObject playerListPrefab;                               // Player List Prefab GameObject Reference
    [SerializeField] private GameObject playerListParent;                               // Player List Parent Reference
    [SerializeField] private GameObject startGameButton;                                // Start Game Button Reference

    // Private Variables
    private Dictionary<string, RoomInfo> cachedRoomList;                                // Cached Room List Dictionary
    private Dictionary<string, GameObject> roomListGameObjects;                         // Room List GameObject Dictionary
    private Dictionary<int, GameObject> playerListGameObjects;                          // Player List GameObject Dictionary

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        PanelManager.Instance.ActivatePanel("login-panel");
        AudioManager.Instance.Play("lobby-bgm");
        PhotonNetwork.AutomaticallySyncScene = true;
        playerNameInput.characterLimit = 10;
        roomNameInputField.characterLimit = 20;
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameObjects = new Dictionary<string, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Press Enter to Login
        foreach(KeyCode keyCodes in enterInputs)
        {
            if (Input.GetKeyUp(keyCodes) && !PhotonNetwork.IsConnected)
            {
                OnLoginButtonClicked();
                break;
            }
        }
    }
    #endregion

    #region UI Callback Methods
    #region Login Functions
    /// <summary>
    /// Login Button
    /// </summary>
    public void OnLoginButtonClicked()
    {
        string playerName = playerNameInput.text;

        // Proceed to connection on photon servers and the internet
        // if player input a name
        if (!string.IsNullOrEmpty(playerName))
        {
            PanelManager.Instance.ActivatePanel("connecting-panel");

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            // Render invalid if player did not put anything
            Debug.Log("PlayerName is invalid!");
        }
    }

    /// <summary>
    /// Return Button
    /// </summary>
    public void OnReturnButtonClicked()
    {
        StartCoroutine(SceneLoader.Instance.LoadScene("MainMenuScene"));
    }
    #endregion

    #region Game Options Functions
    /// <summary>
    /// Create Room Button
    /// </summary>
    public void OnCreateRoomButtonClicked()
    {
        // Go to Create Room Panel
        PanelManager.Instance.ActivatePanel("create-room-panel");
    }

    /// <summary>
    /// Show Room List Button
    /// </summary>
    public void OnShowRoomListButtonClicked()
    {
        // Join Lobby
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        // Go to Room List Panel
        PanelManager.Instance.ActivatePanel("show-room-list-panel");
    }

    /// <summary>
    /// Logout button function
    /// </summary>
    public void OnLogoutButtonClicked()
    {
        // Leave Lobby and disconnect to servers
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        // Disconnect to Servers
        PhotonNetwork.Disconnect();

        // Return back to login panel
        PanelManager.Instance.ActivatePanel("login-panel");
    }
    #endregion

    #region Create Room Functions
    /// <summary>
    /// Create Button
    /// Execute When Player is Done Setting Properties for the Room
    /// </summary>
    public void OnCreateButtonClicked()
    {
        // Create Room if Player has Set Amount of Maximum Players
        if (maxPlayers != 0)
        {
            // Proceed to Creating room
            PanelManager.Instance.ActivatePanel("creating-panel");

            string roomName = roomNameInputField.text;

            // If No Input for Room Name, Generate "Room" and Add Random Number
            if (string.IsNullOrEmpty(roomName))
            {
                roomName = "Room " + Random.Range(1000, 10000);
            }

            // Set Maximum Players
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = (byte)maxPlayers;

            // Create the Room
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }
        else
        {
            // Declare invalid
            Debug.Log("Invalid Inputs!");
            return;
        }
    }

    /// <summary>
    /// Cancel Button
    /// </summary>
    public void OnCancelButtonClicked()
    {
        // Return Back to Game Options Panel
        PanelManager.Instance.ActivatePanel("game-options-panel");
    }
    #endregion

    #region Show Room List Functions
    /// <summary>
    /// Back Button
    /// </summary>
    public void OnBackButtonClicked()
    {
        // Leave Lobby
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        // Return Back to Game Options Panel
        PanelManager.Instance.ActivatePanel("game-options-panel");
    }
    #endregion

    #region Inside Room Functions
    /// <summary>
    /// Leave Game Upon Click
    /// </summary>
    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// Start Game Upon Click
    /// </summary>
    public void OnStartGameButtonClicked()
    {
        // Close and Hide Room
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        // Redirect All Players to GameScene
        PhotonNetwork.LoadLevel("GameScene");
    }
    #endregion
    #endregion

    #region Photon Callbacks
    /// <summary>
    /// Checks if Player is Connected to the Internet
    /// </summary>
    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    /// <summary>
    /// Checks if Player has Connected to the Photon Servers
    /// </summary>
    public override void OnConnectedToMaster()
    {
        // Indicate Connection to Photon Servers
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon");
        PanelManager.Instance.ActivatePanel("game-options-panel");
    }

    /// <summary>
    /// Execute When Player has Disconnected to Photon Servers
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " has disconnected to Photon");
    }

    /// <summary>
    /// Indicates Created Room
    /// </summary>
    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " created!");
    }

    /// <summary>
    /// When the Player Has Joined the Room
    /// </summary>
    public override void OnJoinedRoom()
    {
        // Indicate Player has Joined the Room, and Show Player Count
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " has joined " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);

        // Redirect to Inside Room Panel
        PanelManager.Instance.ActivatePanel("inside-room-panel");

        // Initialize Room Properties (Room Name, and Player Count)
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        playerCountText.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

        // Initialize Player List GameObjects
        if (playerListGameObjects == null)
        {
            playerListGameObjects = new();
        }

        // Call this for Every Player in the Photon Player List
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // Spawn Player List Item
            GameObject playerListItem = Instantiate(playerListPrefab);
            playerListItem.transform.SetParent(playerListParent.transform);
            playerListItem.transform.localScale = Vector3.one;

            // Initialize Player List Item
            playerListItem.GetComponent<PlayerListItemInitializer>().Initialize(player.ActorNumber, player.NickName);

            object isPlayerReady;

            // Update the Set Player Ready Function
            if (player.CustomProperties.TryGetValue(Constants.PLAYER_READY, out isPlayerReady))
            {
                playerListItem.GetComponent<PlayerListItemInitializer>().SetPlayerReady((bool)isPlayerReady);
            }

            // Add Item to List
            playerListGameObjects.Add(player.ActorNumber, playerListItem);
        }

        // Disable Start Game Button
        startGameButton.SetActive(false);
    }

    /// <summary>
    /// Updates Room List
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListGameObjects();

        Debug.Log("OnRoomListUpdate called");

        foreach (RoomInfo info in roomList)
        {
            Debug.Log(info.Name);

            // Remove from List if Room is Hidden or Closed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }
            }
            else
            {
                // Update Existing Rooms Info
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList[info.Name] = info;
                }
                else
                {
                    cachedRoomList.Add(info.Name, info);
                }
            }
        }

        // Add Room List Items for evey Room Created
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            // Spawn Room List Item
            GameObject listItem = Instantiate(roomListItemPrefab);
            listItem.transform.SetParent(roomListParent.transform);
            listItem.transform.localScale = Vector3.one;

            // Initialize Room List Item
            listItem.transform.Find("RoomNameText").GetComponent<TextMeshProUGUI>().text = info.Name;
            listItem.transform.Find("PlayerCountText").GetComponent<TextMeshProUGUI>().text = "Player Count: " + info.PlayerCount + " / " + info.MaxPlayers;
            listItem.transform.Find("JoinButton").GetComponent<Button>().onClick.AddListener(() => OnJoinRoomClicked(info.Name));

            // Add to List
            roomListGameObjects.Add(info.Name, listItem);
        }
    }

    /// <summary>
    /// Checks if Player has Left the Lobby
    /// </summary>
    public override void OnLeftLobby()
    {
        // Leave lobby and Clear Room List Game Objects
        Debug.Log("You Left the Lobby");
        ClearRoomListGameObjects();
        cachedRoomList.Clear();
    }

    /// <summary>
    /// Checks if Another Player has Entered the Room
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Spawn Player List Item
        GameObject playerListItem = Instantiate(playerListPrefab);
        playerListItem.transform.SetParent(playerListParent.transform);
        playerListItem.transform.localScale = Vector3.one;

        // Initialize Player List Item
        playerListItem.GetComponent<PlayerListItemInitializer>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        // Add to List
        playerListGameObjects.Add(newPlayer.ActorNumber, playerListItem);

        // Update Inside Room Properties
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        playerCountText.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

        // Check if All Players are Ready
        startGameButton.SetActive(CheckAllPlayerReady());
    }

    /// <summary>
    /// Check if Another Player has Left the Room
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Update Room Properties
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        playerCountText.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

        // Destroy player list item of the player who left
        Destroy(playerListGameObjects[otherPlayer.ActorNumber].gameObject);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);

        // Check if All Players are Ready
        startGameButton.SetActive(CheckAllPlayerReady());
    }

    /// <summary>
    /// Checks if Player Himself has Left the Room
    /// </summary>
    public override void OnLeftRoom()
    {
        // Destroy All Player List Items
        foreach (GameObject playerlistGameObject in playerListGameObjects.Values)
        {
            Destroy(playerlistGameObject);
        }

        // Clear Player List
        playerListGameObjects.Clear();
        playerListGameObjects = null;

        // Redirect to Game Options Panel
        PanelManager.Instance.ActivatePanel("game-options-panel");
    }

    /// <summary>
    /// Updates Custom Player Properties
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        GameObject playerListGameObject;

        if (playerListGameObjects.TryGetValue(targetPlayer.ActorNumber, out playerListGameObject))
        {
            object isPlayerReady;

            // Update Player Ready Function
            if (changedProps.TryGetValue(Constants.PLAYER_READY, out isPlayerReady))
            {
                playerListGameObject.GetComponent<PlayerListItemInitializer>().SetPlayerReady((bool)isPlayerReady);
            }
        }

        startGameButton.SetActive(CheckAllPlayerReady());
    }

    /// <summary>
    /// Checks if Host has Been Changed
    /// </summary>
    /// <param name="newMasterClient"></param>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            startGameButton.SetActive(CheckAllPlayerReady());
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Sets Up the Maximum Number of Players for the Room
    /// </summary>
    /// <param name="value"></param>
    public void SetMaxPlayers(int value)
    {
        // Revert back to 0 if Value is the Same
        if (maxPlayers == value)
        {
            maxPlayers = 0;
        }
        // Set maxPlayers to the Assigned Value
        else
        {
            maxPlayers = value;
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Join Room
    /// </summary>
    /// <param name="roomName"></param>
    void OnJoinRoomClicked(string roomName)
    {
        // Leave the Lobby, then Join the Room
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        PhotonNetwork.JoinRoom(roomName);
    }

    /// <summary>
    /// Clears Every Room List GameObject
    /// </summary>
    void ClearRoomListGameObjects()
    {
        foreach (var item in roomListGameObjects.Values)
        {
            Destroy(item);
        }

        roomListGameObjects.Clear();
    }

    /// <summary>
    /// Check if All Players are Ready
    /// </summary>
    /// <returns></returns>
    bool CheckAllPlayerReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;

            if (p.CustomProperties.TryGetValue(Constants.PLAYER_READY, out isPlayerReady))
            {
                if (!(bool)isPlayerReady)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }
    #endregion
}