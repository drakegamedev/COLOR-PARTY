using UnityEngine;
using Photon.Pun;

public class Tile : MonoBehaviourPunCallbacks
{
    public GameObject PlayerStep { get; set; }                      // Player Prefab Reference

    // Private Variables
    private SpriteRenderer spriteRenderer;                          // Sprite Renderer Reference
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Don't Execute Code if Player Color is the Same As with the Current Tile Color
            if (spriteRenderer.color == collider.GetComponent<PlayerSetup>().PlayerColor)
                return;

            // Decrease Score of player designated in the color
            if (spriteRenderer.color != Color.white)
            {
                // If player is not Null
                if (PlayerStep != null)
                {
                    // Decrease Score of Previous Player who Stepped on the Tile
                    PlayerStep.GetComponent<PhotonView>().RPC("DecreaseScore", RpcTarget.AllBuffered);
                }
            }

            // Increase Score of Player who Stepped on the Tile
            collider.GetComponent<PhotonView>().RPC("IncreaseScore", RpcTarget.AllBuffered);
            
            // Broadcast Coloring of Tile
            collider.GetComponent<PhotonView>().RPC("ColorTile", RpcTarget.AllBuffered, gameObject.name);
        }
    }
}