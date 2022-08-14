using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Camera PlayerCamera;
    public Color PlayerColor;
    public int PlayerNumber;

    private PlayerMovement playerMovement;
    private PlayerUIController playerUIController;
    private Rigidbody2D rb;
    private Animator animator;

    void OnDisable()
    {
        EventManager.Instance.InitiateGame -= SetPlayerViews;
        EventManager.Instance.EndGame -= DisableMovement;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerUIController = GetComponent<PlayerUIController>();
        rb = GetComponent<Rigidbody2D>();

        // Set Camera
        PlayerCamera.GetComponent<Camera>().enabled = photonView.IsMine;
        PlayerCamera.GetComponent<AudioListener>().enabled = photonView.IsMine;

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
        }
        else
        {
            playerMovement.enabled = false;
            Destroy(rb);
        }

        animator.enabled = true;
    }

    // Disable Player Movement
    public void DisableMovement()
    {
        playerMovement.enabled = false;
    }
}
