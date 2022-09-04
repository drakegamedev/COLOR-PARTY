using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerListItemInitializer : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    public TextMeshProUGUI PlayerNameText;
    public Button PlayerReadyButton;
    public Image PlayerReadyImage;

    // Private Variables
    private bool isPlayerReady = false;

    // Initialize Player List Item Properties
    public void Initialize(int playerId, string playerName)
    {
        PlayerNameText.text = playerName;

        if (PhotonNetwork.LocalPlayer.ActorNumber != playerId)
        {
            // Disable Player Ready Button
            PlayerReadyButton.gameObject.SetActive(false);
        }
        else
        {
            // Sets custom property for each player "isPlayerReady"
            ExitGames.Client.Photon.Hashtable initializeProperties = new ExitGames.Client.Photon.Hashtable() { { Constants.PLAYER_READY, isPlayerReady } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initializeProperties);

            // On Player Ready Button Clicked
            PlayerReadyButton.onClick.AddListener(() =>
            {
                isPlayerReady = !isPlayerReady;
                SetPlayerReady(isPlayerReady);

                // Update Property to the Server
                ExitGames.Client.Photon.Hashtable newProperties = new ExitGames.Client.Photon.Hashtable() { { Constants.PLAYER_READY, isPlayerReady } };

                PhotonNetwork.LocalPlayer.SetCustomProperties(newProperties);
            });
        }
    }

    // Player Ready Setter
    public void SetPlayerReady(bool playerReady)
    {
        PlayerReadyImage.enabled = playerReady;

        if (playerReady)
        {
            PlayerReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "CANCEL";
        }
        else
        {
            PlayerReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "READY";
        }
    }
}