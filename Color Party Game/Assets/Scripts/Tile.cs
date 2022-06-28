using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tile : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<PhotonView>().RPC("ColorTile", RpcTarget.AllBuffered, gameObject.name);
        }
    }
}
