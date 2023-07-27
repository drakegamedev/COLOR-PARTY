using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [field : SerializeField] public float Speed { get; set; }                                // Default Movement Speed
    public float CurrentMoveSpeed { get; set; }                                              // Current Movement Speed

    // Private Variables
    private Rigidbody2D rb;                                                                  // Rigidbody2D Component Reference
    private Animator animator;                                                               // Animator Component Reference
    private Vector2 moveVelocity;                                                            // Movement Velocity

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        CurrentMoveSpeed = Speed;
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

    /// <summary>
    /// Player Top Down Movement
    /// </summary>
    void Movement()
    {
        // Horizontal and Vertical Movement
        float xMovement = Input.GetAxisRaw("Horizontal");
        float yMovement = Input.GetAxisRaw("Vertical");

        Vector2 move = new Vector2(xMovement, yMovement);

        // Normalize Movement Velocity
        moveVelocity = move.normalized * CurrentMoveSpeed;

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
