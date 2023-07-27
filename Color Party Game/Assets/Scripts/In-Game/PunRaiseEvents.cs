using Photon.Pun;
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

    /// <summary>
    /// Event that will occur based on SetRaiseEvent()
    /// </summary>
    /// <param name="photonEvent"></param>
    public virtual void OnEvent(EventData photonEvent)
    {

    }

    /// <summary>
    /// Sets Up RaiseEvent
    /// </summary>
    public virtual void SetRaiseEvent()
    {
        
    }
}
