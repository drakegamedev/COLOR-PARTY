using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public TMP_InputField PlayerNameInput;
    public KeyCode[] EnterInputs;

    [Header("Create Room Panel")]
    public TMP_InputField RoomNameInputField;
    private int maxPlayers;

    [Header("Show Room List Panel")]
    public GameObject RoomListItemPrefab;
    public GameObject RoomListParent;

    [Header("Inside Room Panel")]
    public TextMeshProUGUI RoomNameText;
    public TextMeshProUGUI PlayerCountText;
    public GameObject PlayerListPrefab;
    public GameObject PlayerListParent;
    public GameObject StartGameButton;

    // Private Variables
    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListGameObjects;
    private Dictionary<int, GameObject> playerListGameObjects;

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        PanelManager.Instance.ActivatePanel("login-panel");
        // AudioManager.Instance.Play("lobby-bgm");
        PhotonNetwork.AutomaticallySyncScene = true;
        PlayerNameInput.characterLimit = 10;
        RoomNameInputField.characterLimit = 20;
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameObjects = new Dictionary<string, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Press Enter to Login
        foreach(KeyCode keyCodes in EnterInputs)
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
    // Login Button
    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;

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

    // Return Button
    public void OnReturnButtonClicked()
    {
        StartCoroutine(SceneLoader.Instance.LoadScene("MainMenuScene"));
    }
    #endregion

    #region Game Options Functions
    // Create Room Button
    public void OnCreateRoomButtonClicked()
    {
        // Go to Create Room Panel
        PanelManager.Instance.ActivatePanel("create-room-panel");
    }

    // Show Room List Button
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

    // Logout button function
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
    // Create Button
    // Execute When Player is Done Setting Properties for the Room 
    public void OnCreateButtonClicked()
    {
        // Create Room if Player has Set Amount of Maximum Players
        if (maxPlayers != 0)
        {
            // Proceed to Creating room
            PanelManager.Instance.ActivatePanel("creating-panel");

            string roomName = RoomNameInputField.text;

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

    // Cancel Button
    public void OnCancelButtonClicked()
    {
        // Return Back to Game Options Panel
        PanelManager.Instance.ActivatePanel("game-options-panel");
    }
    #endregion

    #region Show Room List Functions
    // Back Button
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
    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

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
    // Checks if Player is Connected to the Internet
    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    // Checks if Player has Connected to the Photon Servers
    public override void OnConnectedToMaster()
    {
        // Indicate Connection to Photon Servers
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon");
        PanelManager.Instance.ActivatePanel("game-options-panel");
    }

    // Execute When Player has Disconnected to Photon Servers
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " has disconnected to Photon");
    }

    // Indicates Created Room
    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " created!");
    }

    // When the Player Has Joined the Room
    public override void OnJoinedRoom()
    {
        // Indicate Player has Joined the Room, and Show Player Count
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " has joined " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);

        // Redirect to Inside Room Panel
        PanelManager.Instance.ActivatePanel("inside-room-panel");

        // Initialize Room Properties (Room Name, and Player Count)
        RoomNameText.text = PhotonNetwork.CurrentRoom.Name;
        PlayerCountText.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

        // Initialize Player List GameObjects
        if (playerListGameObjects == null)
        {
            playerListGameObjects = new();
        }

        // Call this for Every Player in the Photon Player List
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // Spawn Player List Item
            GameObject playerListItem = Instantiate(PlayerListPrefab);
            playerListItem.transform.SetParent(PlayerListParent.transform);
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
        StartGameButton.SetActive(false);
    }

    // Updates Room List
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
            GameObject listItem = Instantiate(RoomListItemPrefab);
            listItem.transform.SetParent(RoomListParent.transform);
            listItem.transform.localScale = Vector3.one;

            // Initialize Room List Item
            listItem.transform.Find("RoomNameText").GetComponent<TextMeshProUGUI>().text = info.Name;
            listItem.transform.Find("PlayerCountText").GetComponent<TextMeshProUGUI>().text = "Player Count: " + info.PlayerCount + " / " + info.MaxPlayers;
            listItem.transform.Find("JoinButton").GetComponent<Button>().onClick.AddListener(() => OnJoinRoomClicked(info.Name));

            // Add to List
            roomListGameObjects.Add(info.Name, listItem);
        }
    }

    // Checks if Player has Left the Lobby
    public override void OnLeftLobby()
    {
        // Leave lobby and Clear Room List Game Objects
        Debug.Log("You Left the Lobby");
        ClearRoomListGameObjects();
        cachedRoomList.Clear();
    }

    // Checks if Another Player has Entered the Room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Spawn Player List Item
        GameObject playerListItem = Instantiate(PlayerListPrefab);
        playerListItem.transform.SetParent(PlayerListParent.transform);
        playerListItem.transform.localScale = Vector3.one;

        // Initialize Player List Item
        playerListItem.GetComponent<PlayerListItemInitializer>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        // Add to List
        playerListGameObjects.Add(newPlayer.ActorNumber, playerListItem);

        // Update Inside Room Properties
        RoomNameText.text = PhotonNetwork.CurrentRoom.Name;
        PlayerCountText.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

        // Check if All Players are Ready
        StartGameButton.SetActive(CheckAllPlayerReady());
    }

    // Check if Another Player has Left the Room
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Update Room Properties
        RoomNameText.text = PhotonNetwork.CurrentRoom.Name;
        PlayerCountText.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

        // Destroy player list item of the player who left
        Destroy(playerListGameObjects[otherPlayer.ActorNumber].gameObject);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);

        // Check if All Players are Ready
        StartGameButton.SetActive(CheckAllPlayerReady());
    }

    // Checks if Player Himself has Left the Room
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

    // Updates Custom Player Properties
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

        StartGameButton.SetActive(CheckAllPlayerReady());
    }

    // Checks if Host has Been Changed
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            StartGameButton.SetActive(CheckAllPlayerReady());
        }
    }
    #endregion

    #region Public Methods
    // Sets Up the Maximum Number of Players for the Room
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
    // Join Room
    void OnJoinRoomClicked(string roomName)
    {
        // Leave the Lobby, then Join the Room
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        PhotonNetwork.JoinRoom(roomName);
    }

    // Clears Every Room List GameObject
    void ClearRoomListGameObjects()
    {
        foreach (var item in roomListGameObjects.Values)
        {
            Destroy(item);
        }

        roomListGameObjects.Clear();
    }

    // Check if All Players are Ready
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