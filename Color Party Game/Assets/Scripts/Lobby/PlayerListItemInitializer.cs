using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerListItemInitializer : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI playerNameText;                    // Player Name Text Reference
    [SerializeField] private Button playerReadyButton;                          // Player Ready Button Reference
    [SerializeField] private Image playerReadyImage;                            // Player Ready Image Reference

    // Private Variables
    private bool isPlayerReady = false;                                         // Indicates in Player is Ready or Not

    /// <summary>
    /// Initialize Player List Item Properties
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="playerName"></param>
    public void Initialize(int playerId, string playerName)
    {
        playerNameText.text = playerName;

        if (PhotonNetwork.LocalPlayer.ActorNumber != playerId)
        {
            // Disable Player Ready Button
            playerReadyButton.gameObject.SetActive(false);
        }
        else
        {
            // Sets custom property for each player "isPlayerReady"
            ExitGames.Client.Photon.Hashtable initializeProperties = new ExitGames.Client.Photon.Hashtable() { { Constants.PLAYER_READY, isPlayerReady } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initializeProperties);

            // On Player Ready Button Clicked
            playerReadyButton.onClick.AddListener(() =>
            {
                isPlayerReady = !isPlayerReady;
                SetPlayerReady(isPlayerReady);

                // Update Property to the Server
                ExitGames.Client.Photon.Hashtable newProperties = new ExitGames.Client.Photon.Hashtable() { { Constants.PLAYER_READY, isPlayerReady } };

                PhotonNetwork.LocalPlayer.SetCustomProperties(newProperties);
            });
        }
    }

    /// <summary>
    /// Player Ready Setter
    /// </summary>
    /// <param name="playerReady"></param>
    public void SetPlayerReady(bool playerReady)
    {
        playerReadyImage.enabled = playerReady;

        if (playerReady)
        {
            playerReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "CANCEL";
        }
        else
        {
            playerReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "READY";
        }
    }
}