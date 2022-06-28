using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerScoring : MonoBehaviourPunCallbacks
{
    private PlayerSetup playerSetup;
    private GameObject[] tiles;

    void Start()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        playerSetup = GetComponent<PlayerSetup>();
    }


    [PunRPC]
    // Visual Indication that the player has colored a tile
    public void ColorTile(string tileName)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tileName == tiles[i].name)
            {
                SpriteRenderer tileSprite = tiles[i].GetComponent<SpriteRenderer>();
                tileSprite.color = playerSetup.PlayerColor;
                break;
            }
        }
    }
}
