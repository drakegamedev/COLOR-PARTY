using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Health : MonoBehaviourPunCallbacks
{
    [SerializeField] private float respawnTime;                                           // Respawn Timer
    [SerializeField] private SpriteRenderer[] playerSprites;                              // Player Graphic Sprite Reference
    [SerializeField] private Color deathColor;                                            // Player Death Color
    [SerializeField] private Color lifeColor;                                             // Player Life Color
    
    public bool IsAlive { get; private set; }                                             // Indicator if Player is Alive

    // Private Variables
    private PlayerMovement playerMovement;                                                // PlayerMovement Class Reference
    private PlayerSetup playerSetup;                                                      // PlayerSetup Class Reference

    // Start is called before the first frame update
    void Start()
    {
        IsAlive = true;
        playerMovement = GetComponent<PlayerMovement>();
        playerSetup = GetComponent<PlayerSetup>();
    }

    /// <summary>
    /// Player Death
    /// </summary>
    [PunRPC]
    public void OnDeath()
    {
        StartCoroutine(Respawn());
    }

    /// <summary>
    /// Respawn Timer
    /// </summary>
    /// <returns></returns>
    IEnumerator Respawn()
    {
        AudioManager.Instance.Play("explosion-sfx");
        Poolable explosion = ObjectPooler.Instance.SpawnFromPool("explosion", transform.position, Quaternion.identity).GetComponent<Poolable>();
        DeathEffect();

        yield return new WaitForSeconds(respawnTime);

        explosion.ReturnToPool();
        LifeEffect();
    }

    #region VFX
    // Visual Indicator for Player Death
    void DeathEffect()
    {
        IsAlive = false;
        playerMovement.enabled = false;

        foreach (SpriteRenderer sr in playerSprites)
        {
            sr.color = deathColor;
        }
    }

    // Visual Indicator for Player Respawn
    void LifeEffect()
    {
        IsAlive = true;
        transform.position = PlayerSpawnManager.Instance.SpawnPoints[playerSetup.PlayerNumber - 1].position;
        playerMovement.enabled = photonView.IsMine;

        foreach (SpriteRenderer sr in playerSprites)
        {
            sr.color = lifeColor;
        }
    }
    #endregion
}
