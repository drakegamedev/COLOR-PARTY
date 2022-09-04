using System.Collections.Generic;
using UnityEngine;

// Generates Pool of Objects for Optimization
public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    
    [System.Serializable]
    public struct Pool
    {
        public string Id;
        public GameObject Prefab;
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
        // Initialize Pools
        foreach (Pool pool in Pools)
        {
            List<GameObject> objectPool = new();
            PoolDictionary.Add(pool.Id, objectPool);
        }
    }

    // Spawns an Object from the Pool
    public GameObject SpawnFromPool(string id, Vector3 position, Quaternion rotation)
    {
        if (!PoolDictionary.ContainsKey(id))
        {
            Debug.LogWarning("Pool with tag " + id + " doesn't exist.");
            return null;
        }

        // Recycle Object
        foreach (GameObject go in PoolDictionary[id])
        {
            if (!go.activeInHierarchy)
            {
                GameObject recycledObject = go;
                recycledObject.SetActive(true);

                // Remove from Pool, then Set Position and Rotation
                recycledObject.transform.SetParent(null);
                recycledObject.transform.position = position;
                recycledObject.transform.rotation = rotation;

                return recycledObject;
            }
        }

        // Spawn New Object
        foreach (Pool objPool in Pools)
        {
            if (objPool.Id == id)
            {
                // Spawn object and add poolable component
                GameObject newObject = Instantiate(objPool.Prefab);

                // Set Id if Object is a Power-Up
                if (newObject.GetComponent<PowerUps>() != null)
                {
                    newObject.GetComponent<PowerUps>().Id = id;
                }
                
                newObject.AddComponent<Poolable>();

                // Set Position and Rotation
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;

                // Add Gameobject to List
                PoolDictionary[id].Add(newObject);

                newObject.transform.parent = null;

                // Initialize Name
                newObject.transform.name = newObject.transform.name + PoolDictionary[id].Count.ToString();

                return newObject;
            }
        }

        return null;
    }
}