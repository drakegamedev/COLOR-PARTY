using UnityEngine;
using Photon.Pun;

// Abstract Power-Up Class
public class PowerUps : MonoBehaviourPunCallbacks
{
    public string Id { get; set; }                          // Power-Up ID
    public float DestructorTime;                            // Destructor Timer

    // Private Variables
    private float currentDestructorTime;                    // Current Destructor Time
    private Poolable poolable;                              // Poolable Class Reference

    public override void OnEnable()
    {
        currentDestructorTime = DestructorTime;
    }

    void Start()
    {
        poolable = GetComponent<Poolable>();
    }

    void Update()
    {
        // Initiate Destructor Timer when Game has Started
        if (currentDestructorTime > 0f && GameManager.Instance.GameState == GameManager.GameStates.PLAYING)
        {
            currentDestructorTime -= Time.deltaTime;
        }
        else
        {
            currentDestructorTime = DestructorTime;
            poolable.ReturnToPool();
        }
    }

    // Power-Up Effect
    public virtual void TakeEffect(Collider2D collider)
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Despawner despawner = collider.GetComponent<Despawner>();

            AudioManager.Instance.Play("pick-up-sfx");

            TakeEffect(collider);

            despawner.ObjectName = gameObject.name;
            despawner.ObjectID = Id;
            despawner.SetRaiseEvent();
        }
    }
}
