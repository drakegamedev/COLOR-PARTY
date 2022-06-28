using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tile : MonoBehaviourPunCallbacks
{
    public GameObject PlayerStep { get { return playerStep; } set { playerStep = value; } }
    
    private SpriteRenderer spriteRenderer;
    private GameObject playerStep;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (spriteRenderer.color == collider.GetComponent<PlayerSetup>().PlayerColor)
            {
                Debug.Log("Color is the same");
                return;
            }
            else
            {
                // Decrease Score of player designated in the color
                if (spriteRenderer.color != Color.white)
                {
                    Debug.Log("Replace Color!");
                    playerStep.GetComponent<PhotonView>().RPC("DecreaseScore", RpcTarget.AllBuffered);
                }

                Debug.Log("New Color!");
                collider.GetComponent<PhotonView>().RPC("IncreaseScore", RpcTarget.AllBuffered);
                collider.GetComponent<PhotonView>().RPC("ColorTile", RpcTarget.AllBuffered, gameObject.name);
            }
        }
    }
}
