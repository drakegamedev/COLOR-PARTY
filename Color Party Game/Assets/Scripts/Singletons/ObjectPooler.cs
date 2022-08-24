using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    
    [System.Serializable]
    public struct Pool
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
    public Dictionary<string, List<GameObject>> PoolDictionary { get; } = new();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Pool pool in Pools)
        {
            List<GameObject> objectPool = new();

            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab);
                obj.transform.parent = transform;
                obj.transform.name = obj.transform.name + i.ToString();
                obj.SetActive(false);
                objectPool.Add(obj);
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

        // Recycle Object
        for (int i = 0; i < PoolDictionary[id].Count; i++)
        {
            if (!PoolDictionary[id][i].activeInHierarchy)
            {
                GameObject spawnObject = PoolDictionary[id][i];
                spawnObject.SetActive(true);

                spawnObject.transform.SetParent(null);
                spawnObject.transform.position = position;
                spawnObject.transform.rotation = rotation;

                return spawnObject;
            }
        }

        // Spawn New Object
        foreach (Pool objPool in Pools)
        {
            if (objPool.Id == id)
            {
                // Spawn object and add poolable component
                GameObject objectToSpawn = Instantiate(objPool.Prefab);
                objectToSpawn.GetComponent<PowerUps>().Id = id;
                objectToSpawn.AddComponent<Poolable>();

                // Set Position and Rotation
                objectToSpawn.transform.position = position;
                objectToSpawn.transform.rotation = rotation;

                // Add gameobject to list
                PoolDictionary[id].Add(objectToSpawn);

                objectToSpawn.transform.parent = null;

                // Initialize Name
                objectToSpawn.transform.name = objectToSpawn.transform.name + PoolDictionary[id].Count.ToString();

                return objectToSpawn;
            }
        }

        return null;
    }
}