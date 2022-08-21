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
        INITIAL_COUNTDOWN,
        TIMER,
        TIME_UP,
        PLAYER_WINNER
    }
    
    public virtual void OnEvent(EventData photonEvent)
    {

    }

    public virtual void SetRaiseEvent()
    {
        
    }
}
