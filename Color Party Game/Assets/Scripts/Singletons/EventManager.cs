using UnityEngine;
using System;

// Manages All Event Systems
public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public Action DisableCountdown;                                     // Disables Countdown Timer
    public Action InitiateGame;                                         // Initiates the Game Session
    public Action EndGame;                                              // Ends the Game Session
    public Action Intensify;                                            // Instensifies the Game's Atmosphere

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
