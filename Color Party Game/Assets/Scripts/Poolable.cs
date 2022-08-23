using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    public void ReturnToPool()
    {
        transform.SetParent(ObjectPooler.Instance.transform);
        gameObject.SetActive(false);
    }
}
