using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    
    [System.Serializable]
    public class Pool
    {
        public string Id;
        public GameObject Prefab;
        public int Size;
    }

    #region Singleton
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary { get; } = new();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Pool pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab);
                obj.transform.parent = transform;
                obj.transform.name = obj.transform.name + i.ToString();
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            PoolDictionary.Add(pool.Id, objectPool);
        }
    }

    public GameObject SpawnFromPool(string id, Vector3 position, Quaternion rotation)
    {
        if (!PoolDictionary.ContainsKey(id))
        {
            Debug.LogWarning("Pool with tag " + id + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = PoolDictionary[id].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        PoolDictionary[id].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}