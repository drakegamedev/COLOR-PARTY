using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

// Abstract Class for Syncronized Raising of Events 
public class PunRaiseEvents : MonoBehaviourPunCallbacks
{
    // Raise Events Code
    public enum RaiseEvents
    {
        // Time Ststes
        INITIAL_COUNTDOWN,
        TIMER,
        TIME_UP,

        // Spawning & Despawning
        SPAWN_POWER_UP,
        DESPAWN_POWER_UP,

        // Results
        PLAYER_WINNER
    }
    
    // Event that will occur based on SetRaiseEvent()
    public virtual void OnEvent(EventData photonEvent)
    {

    }

    // Sets Up RaiseEvent
    public virtual void SetRaiseEvent()
    {
        
    }
}
