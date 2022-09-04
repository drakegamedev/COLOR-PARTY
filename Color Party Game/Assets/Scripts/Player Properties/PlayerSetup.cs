using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Camera PlayerCamera;
    public Color PlayerColor;
    public int PlayerNumber;

    private PlayerMovement playerMovement;
    private PlayerUIController playerUIController;
    private Despawner despawner;
    private WinLoseIndicator winLoseIndicator;
    private Rigidbody2D rb;
    private Animator animator;

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

        // Set player name
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
