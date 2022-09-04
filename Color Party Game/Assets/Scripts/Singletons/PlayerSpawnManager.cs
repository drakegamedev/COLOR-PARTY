using System.Collections.Generic;
using UnityEngine;

// Holds All Player Spawn Points
public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance;
    public List<Transform> SpawnPoints { get; private set; } = new();

    #region Singleton
    void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Get All Spawn Point Child Objects
        for (int i = 0; i < transform.childCount; i++)
        {
            SpawnPoints.Add(transform.GetChild(i));
        }
    }
    #endregion
}
