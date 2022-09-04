using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Camera PlayerCamera;                                     // Player Camera
    public Color PlayerColor;                                       // Player Color
    public int PlayerNumber;                                        // Player Number

    // Private Variables
    private PlayerMovement playerMovement;                          // PlayerMovement Class Reference
    private PlayerUIController playerUIController;                  // PlayerUIController Class Reference
    private Despawner despawner;                                    // Despawner Class Reference
    private WinLoseIndicator winLoseIndicator;                      // WinLoseIndicator Class Reference
    private Rigidbody2D rb;                                         // Rigidbody2D Component Reference
    private Animator animator;                                      // Animator Component Reference

    public override void OnDisable()
    {
        EventManager.Instance.InitiateGame -= SetPlayerViews;
        EventManager.Instance.EndGame -= DisableMovement;
        GameManager.Instance.PlayerGameObjects.Remove(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerUIController = GetComponent<PlayerUIController>();
        despawner = GetComponent<Despawner>();
        winLoseIndicator = GetComponent<WinLoseIndicator>();
        rb = GetComponent<Rigidbody2D>();

        GameManager.Instance.PlayerGameObjects.Add(gameObject);

        // Set Camera
        PlayerCamera.GetComponent<Camera>().enabled = photonView.IsMine;
        PlayerCamera.GetComponent<AudioListener>().enabled = photonView.IsMine;

        // Set Animator Controller
        animator.SetBool("isLocalPlayer", photonView.IsMine);

        // Disable Movement and Animation
        animator.enabled = true;
        playerMovement.enabled = false;

        // Set Player Name
        playerUIController.PlayerNameText.text = photonView.Owner.NickName;

        EventManager.Instance.InitiateGame += SetPlayerViews;
        EventManager.Instance.EndGame += DisableMovement;
    }

    // Activate Scripts based on Photon View
    public void SetPlayerViews()
    {
        if (photonView.IsMine)
        {
            playerMovement.enabled = true;
            winLoseIndicator.enabled = true;
            despawner.enabled = true;
        }
        else
        {
            playerMovement.enabled = false;
            winLoseIndicator.enabled = false;
            despawner.enabled = false;
            Destroy(rb);
        }

        // Enable animations for all players
        animator.enabled = true;
    }

    // Disable Player Movement
    public void DisableMovement()
    {
        playerMovement.enabled = false;
    }
}
