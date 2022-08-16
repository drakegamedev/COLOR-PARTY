using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerScoring : MonoBehaviourPunCallbacks
{
    private int playerScore;
    private PlayerSetup playerSetup;
    private PlayerUIController playerUIController;
    private GameObject[] tiles;

    // Start is called before the first frame update
    void Start()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        playerSetup = GetComponent<PlayerSetup>();
        playerUIController = GetComponent<PlayerUIController>();
        playerScore = 0;
    }


    // Increase Player Score
    [PunRPC]
    public void IncreaseScore()
    {
        playerScore++;
        //UpdateToScoreItem();
        photonView.RPC("UpdateToScoreItem", RpcTarget.AllBuffered);
    }

    // Decrease Player Score
    [PunRPC]
    public void DecreaseScore()
    {
        playerScore--;

        if (playerScore < 0)
        {
            playerScore = 0;
        }

        //UpdateToScoreItem();
        photonView.RPC("UpdateToScoreItem", RpcTarget.AllBuffered);
    }

    // Update player score to Score UI
    [PunRPC]
    public void UpdateToScoreItem()
    {
        playerUIController.PlayerScoreItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerScore.ToString();
    }

    [PunRPC]
    // Visual Indication that the player has colored a tile
    // Input player gameobject data to tile
    public void ColorTile(string tileName)
    {
        /*for (int i = 0; i < tiles.Length; i++)
        {
            if (tileName == tiles[i].name)
            {
                SpriteRenderer tileSprite = tiles[i].GetComponent<SpriteRenderer>();
                Tile tileScript = tiles[i].GetComponent<Tile>();
                tileSprite.color = playerSetup.PlayerColor;
                tileScript.PlayerStep = gameObject;
                break;
            }
        }*/

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