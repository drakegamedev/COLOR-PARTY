using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerStatus : MonoBehaviourPunCallbacks
{
    public int Place { get; set; }                              // Indicate Player Rank (e.g. 1,2,3, etc.)
    public string OrdinalPlace { get; private set; }            // Add Suffix to Player Rank (e.g. 1st, 2nd, 3rd)
    public float Timer;

    private PlayerMovement playerMovement;

    // Speed Up Variables
    private bool isSpeeding;
    private float currentSpeedUpTime;
    public GameObject SpeedUpEffect;

    // Slow Down Variables
    private bool isSlowing;
    private float currentSlowDownTime;
    public GameObject SlowDownEffect;

    // Knock Out Variables
    private bool canKill;
    private float currentKnockOutTime;
    public GameObject KnockOutEffect;

    // Freeze 'Em Variables
    private bool isFrozen;
    private float currentFreezeTime;
    private float freezeTime;

    public override void OnDisable()
    {
        EventManager.Instance.EndGame -= AddToScoreList;
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.EndGame += AddToScoreList;
        playerMovement = GetComponent<PlayerMovement>();
        isSpeeding = false;
        isSlowing = false;
        canKill = false;
        isFrozen = false;
        currentSpeedUpTime = Timer;
        currentSlowDownTime = Timer;
        currentKnockOutTime = Timer;
        freezeTime = Timer - 7f;
        currentFreezeTime = freezeTime;
    }

    #region StatusEffects
    #region SpeedUp
    [PunRPC]
    // Speed Up Functions
    public void SpeedUp()
    {
        if (!isSpeeding && !isFrozen)
        {
            currentSpeedUpTime = Timer;
            StartCoroutine(Speeding());
        }
        else
        {
            currentSpeedUpTime = Timer;
        }
    }

    IEnumerator Speeding()
    {
        playerMovement.CurrentMoveSpeed = playerMovement.Speed * 2;
        SpeedClamp();
        isSpeeding = true;
        SpeedUpEffect.SetActive(true);

        while (currentSpeedUpTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            currentSpeedUpTime--;
        }

        playerMovement.CurrentMoveSpeed /= 2;
        SpeedClamp();
        isSpeeding = false;
        SpeedUpEffect.SetActive(false);
    }
    #endregion

    #region SlowDown
    // Slow Down Functions
    [PunRPC]
    public void SlowDown()
    {
        if (!isSlowing && !isFrozen)
        {
            currentSlowDownTime = Timer;
            StartCoroutine(Slowing());
        }
        else
        {
            currentSlowDownTime = Timer;
        }
    }

    IEnumerator Slowing()
    {
        playerMovement.CurrentMoveSpeed = playerMovement.Speed / 2;
        SpeedClamp();
        isSlowing = true;
        SlowDownEffect.SetActive(true);

        while (currentSlowDownTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            currentSlowDownTime--;
        }

        playerMovement.CurrentMoveSpeed *= 2;
        SpeedClamp();
        isSlowing = false;
        SlowDownEffect.SetActive(false);
    }
    #endregion

    #region KnockOut
    // Knock Out Functions
    [PunRPC]
    public void KnockOut()
    {
        if (!canKill)
        {
            currentKnockOutTime = Timer;
            StartCoroutine(Knocking());
        }
        else
        {
            currentKnockOutTime = Timer;
        }
    }

    IEnumerator Knocking()
    {
        canKill = true;
        KnockOutEffect.SetActive(true);

        while (currentKnockOutTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            currentKnockOutTime--;
        }

        canKill = false;
        KnockOutEffect.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && canKill && collider.GetComponent<Health>().IsAlive)
        {
            Debug.Log("EXECUTE");
            collider.GetComponent<PhotonView>().RPC("OnDeath", RpcTarget.AllBuffered);
            currentKnockOutTime = 0;
            canKill = false;
            KnockOutEffect.SetActive(false);
        }
    }
    #endregion

    #region FreezeEm
    [PunRPC]
    public void FreezeEm()
    {
        if (!isFrozen)
        {
            currentFreezeTime = freezeTime;
            StartCoroutine(Frozen());
        }
        else
        {
            currentFreezeTime = freezeTime;
        }
    }

    IEnumerator Frozen()
    {
        playerMovement.CurrentMoveSpeed = 0;
        isFrozen = true;

        while (currentFreezeTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            currentFreezeTime--;
        }

        playerMovement.CurrentMoveSpeed = playerMovement.Speed;
        isFrozen = false;
    }
    #endregion


    // Player Movement Speed Clamp
    void SpeedClamp()
    {
        // Minimum
        if (playerMovement.CurrentMoveSpeed < playerMovement.Speed / 2)
        {
            playerMovement.CurrentMoveSpeed = playerMovement.Speed / 2;
        }
        // Maximum
        else if (playerMovement.CurrentMoveSpeed > playerMovement.Speed * 2)
        {
            playerMovement.CurrentMoveSpeed = playerMovement.Speed * 2;
        }
    }
    #endregion

    #region PlayerStanding
    // Converts Integer to Ordinal Number
    public void Ordinalize(int number)
    {
        // Default suffix
        string numberSuffix = "TH";

        // Get last 2 digits
        int lastTwoDigits = number % 100;

        // Set Suffix as TH, if last 2 digits yields 11, 12, or 13
        if (lastTwoDigits < 11 || lastTwoDigits > 13)
        {
            // Check last digit of the number
            switch (lastTwoDigits % 10)
            {
                case 1:
                    // 1st Suffix
                    numberSuffix = "ST";
                    break;

                case 2:
                    // 2nd Suffix
                    numberSuffix = "ND";
                    break;

                case 3:
                    // 3rd Suffix
                    numberSuffix = "RD";
                    break;
            }
        }

        // Set Ordinal Place
        OrdinalPlace = number + numberSuffix;
    }

    // Shows Ranking of the Player
    public void ShowPlayerRank()
    {
        TextMeshProUGUI playerStandingText = GameManager.Instance.PlayerStanding;

        if (photonView.IsMine)
        {
            playerStandingText.text = "YOU REACHED " + OrdinalPlace + " PLACE.";
        }
    }

    // Add Player To Score List
    public void AddToScoreList()
    {
        ScoreManager.Instance.Players.Add(gameObject);
    }
    #endregion
}
