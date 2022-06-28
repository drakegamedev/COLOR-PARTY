using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance;
    public List<Transform> SpawnPoints { get { return spawnPoints; } set { spawnPoints = value; } }

    private List<Transform> spawnPoints;

    #region Singleton
    void Awake()
    {
        // Singleton Pattern
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        // Get All Spawn Point Child Objects
        spawnPoints = new();

        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints.Add(transform.GetChild(i));
        }
    }
    #endregion
}
