using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private Camera playerCamera;                                     // Player Camera
    [field : SerializeField] public Color PlayerColor { get; private set; }           // Player Color
    [field: SerializeField] public int PlayerNumber { get; private set; }             // Player Number

    // Private Variables
    private PlayerMovement playerMovement;                                            // PlayerMovement Class Reference
    private PlayerUIController playerUIController;                                    // PlayerUIController Class Reference
    private Despawner despawner;                                                      // Despawner Class Reference
    private WinLoseIndicator winLoseIndicator;                                        // WinLoseIndicator Class Reference
    private Rigidbody2D rb;                                                           // Rigidbody2D Component Reference
    private Animator animator;                                                        // Animator Component Reference

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
        playerCamera.GetComponent<Camera>().enabled = photonView.IsMine;
        playerCamera.GetComponent<AudioListener>().enabled = photonView.IsMine;

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
