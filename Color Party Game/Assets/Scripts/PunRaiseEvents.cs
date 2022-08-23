using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PunRaiseEvents : MonoBehaviourPunCallbacks
{
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
    
    public virtual void OnEvent(EventData photonEvent)
    {

    }

    public virtual void SetRaiseEvent()
    {
        
    }
}
