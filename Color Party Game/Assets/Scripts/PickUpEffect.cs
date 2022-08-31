using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpEffect : MonoBehaviour
{
    public float DestructorTime;
    private Poolable poolable;

    void OnEnable()
    {
        StartCoroutine(Destructor());    
    }

    // Start is called before the first frame update
    void Start()
    {
        poolable = GetComponent<Poolable>();
    }

    IEnumerator Destructor()
    {
        yield return new WaitForSeconds(DestructorTime);
        poolable.ReturnToPool();
    }
}
