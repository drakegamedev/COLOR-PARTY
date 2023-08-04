using UnityEngine;

// For objects that will be put in the Object Pooler
public class Poolable : MonoBehaviour
{
    /// <summary>
    /// Returns object back to the Object Pooler
    /// </summary>
    public void ReturnToPool()
    {
        transform.SetParent(ObjectPooler.Instance.transform);
        gameObject.SetActive(false);
    }
}
