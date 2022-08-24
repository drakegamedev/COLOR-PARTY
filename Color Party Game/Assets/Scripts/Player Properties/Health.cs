using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Health : MonoBehaviourPunCallbacks
{
    public float RespawnTime;
    public SpriteRenderer[] PlayerSprites;
    public Color DeathColor;
    public Color LifeColor;
    
    public bool IsAlive { get; private set; }

    private PlayerMovement playerMovement;
    private PlayerSetup playerSetup;

    // Start is called before the first frame update
    void Start()
    {
        IsAlive = true;
        playerMovement = GetComponent<PlayerMovement>();
        playerSetup = GetComponent<PlayerSetup>();
    }

    // Player Death
    [PunRPC]
    public void OnDeath()
    {
        StartCoroutine(Respawn());
    }

    // Respawn Timer
    IEnumerator Respawn()
    {
        DeathEffect();

        yield return new WaitForSeconds(RespawnTime);

        LifeEffect();
    }

    #region VFX
    // Visual Indicator for Player Death
    void DeathEffect()
    {
        IsAlive = false;
        playerMovement.enabled = false;

        foreach (SpriteRenderer sr in PlayerSprites)
        {
            sr.color = DeathColor;
        }
    }

    // Visual Indicator for Player Respawn
    void LifeEffect()
    {
        IsAlive = true;
        transform.position = PlayerSpawnManager.Instance.SpawnPoints[playerSetup.PlayerNumber - 1].position;
        playerMovement.enabled = photonView.IsMine;

        foreach (SpriteRenderer sr in PlayerSprites)
        {
            sr.color = LifeColor;
        }
    }
    #endregion
}
