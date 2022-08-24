using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PowerUps : MonoBehaviourPunCallbacks
{
    public string Id { get; set; }
    public float DestructorTime;

    private float currentDestructorTime;
    private Poolable poolable;

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

    public virtual void TakeEffect(Collider2D collider)
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Despawner despawner = collider.GetComponent<Despawner>();

            TakeEffect(collider);

            despawner.ObjectName = gameObject.name;
            despawner.ObjectID = Id;
            despawner.SetRaiseEvent();
        }
    }
}
