using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public float Speed;

    private Rigidbody2D rb;
    private Animator animator;
    private float currentMoveSpeed;
    private Vector2 moveVelocity;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        currentMoveSpeed = Speed;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    private void Movement()
    {
        // Horizontal and Vertical Movement
        float xMovement = Input.GetAxisRaw("Horizontal");
        float yMovement = Input.GetAxisRaw("Vertical");

        Vector2 move = new Vector2(xMovement, yMovement);

        moveVelocity = move.normalized * currentMoveSpeed;

        // Player Animation
        if (xMovement == 0 && yMovement == 0)
        {
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", true);
        }
    }
}
