using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerScoring : MonoBehaviourPunCallbacks
{
    public int PlayerScore { get; private set; }                            // Player Score

    // Private Variables
    private PlayerSetup playerSetup;                                        // PlayerSetup Class Reference
    private PlayerUIController playerUIController;                          // PlayerUIController Class Reference
    private GameObject[] tiles;                                             // Tile Prefabs Reference

    // Start is called before the first frame update
    void Start()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        playerSetup = GetComponent<PlayerSetup>();
        playerUIController = GetComponent<PlayerUIController>();
        PlayerScore = 0;
    }


    /// <summary>
    /// Increase Player Score
    /// </summary>
    [PunRPC]
    public void IncreaseScore()
    {
        PlayerScore++;
        photonView.RPC("UpdateToScoreItem", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// Decrease Player Score
    /// </summary>
    [PunRPC]
    public void DecreaseScore()
    {
        PlayerScore--;

        // Clamp Player Score
        if (PlayerScore < 0)
        {
            PlayerScore = 0;
        }

        photonView.RPC("UpdateToScoreItem", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// Update player score to Score UI
    /// </summary>
    [PunRPC]
    public void UpdateToScoreItem()
    {
        playerUIController.PlayerScoreItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = PlayerScore.ToString();
    }

    /// <summary>
    /// Visual Indication that the player has colored a tile
    /// Input player gameobject data to tile
    /// </summary>
    /// <param name="tileName"></param>
    [PunRPC] 
    public void ColorTile(string tileName)
    {
        foreach (GameObject tile in tiles)
        {
            if (tile.name == tileName)
            {
                SpriteRenderer tileSprite = tile.GetComponent<SpriteRenderer>();
                Tile tileScript = tile.GetComponent<Tile>();
                tileSprite.color = playerSetup.PlayerColor;
                tileScript.PlayerStep = gameObject;
                break;
            }
        }
    }
}