using UnityEngine;
using System;

// Manages All Event Systems
public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public Action DisableCountdown;
    public Action InitiateGame;
    public Action EndGame;
    public Action Intensify;

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
}
