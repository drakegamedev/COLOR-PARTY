using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerScoring : MonoBehaviourPunCallbacks
{
    public int PlayerScore { get; private set; }
    private PlayerSetup playerSetup;
    private PlayerUIController playerUIController;
    private GameObject[] tiles;

    // Start is called before the first frame update
    void Start()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        playerSetup = GetComponent<PlayerSetup>();
        playerUIController = GetComponent<PlayerUIController>();
        PlayerScore = 0;
    }


    // Increase Player Score
    [PunRPC]
    public void IncreaseScore()
    {
        PlayerScore++;
        photonView.RPC("UpdateToScoreItem", RpcTarget.AllBuffered);
    }

    // Decrease Player Score
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

    // Update player score to Score UI
    [PunRPC]
    public void UpdateToScoreItem()
    {
        playerUIController.PlayerScoreItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = PlayerScore.ToString();
    }

    [PunRPC]
    // Visual Indication that the player has colored a tile
    // Input player gameobject data to tile
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