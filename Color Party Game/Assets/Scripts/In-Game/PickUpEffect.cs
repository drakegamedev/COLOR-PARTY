using System.Collections;
using UnityEngine;

public class PickUpEffect : MonoBehaviour
{
    public float DestructorTime;                    // Destructor Timer
    private Poolable poolable;                      // Poolable Class Reference

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
