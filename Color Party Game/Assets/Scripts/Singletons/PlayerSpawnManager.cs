using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance;
    public Transform[] SpawnPoints { get { return spawnPoints; } set { spawnPoints = value; } }

    private Transform[] spawnPoints;

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
        spawnPoints = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }
    }
    #endregion
}
