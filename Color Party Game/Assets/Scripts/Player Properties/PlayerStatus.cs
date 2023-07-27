using System.Collections;
using UnityEngine;
using TMPro;
using Photon.Pun;

// Manages Status Effects, and Indicates Player Standing
public class PlayerStatus : MonoBehaviourPunCallbacks
{
    public int Place { get; set; }                                  // Indicate Player Rank (e.g. 1,2,3, etc.)
    public string OrdinalPlace { get; private set; }                // Add Suffix to Player Rank (e.g. 1st, 2nd, 3rd)
    [SerializeField] private float timer;                           // Status Effect Duration

    private PlayerMovement playerMovement;                          // PlayerMovement Class Reference

    // Speed Up Variables
    [SerializeField] private GameObject speedUpEffect;              // Speed-Up Effect Reference
    private bool isSpeeding;
    private float currentSpeedUpTime;

    // Slow Down Variables
    [SerializeField] private GameObject slowDownEffect;             // Slow-Down Effect Reference
    private bool isSlowing;
    private float currentSlowDownTime;

    // Knock Out Variables
    [SerializeField] private GameObject knockOutEffect;             // Knock-Out Effect Reference
    private bool canKill;
    private float currentKnockOutTime;

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
        currentSpeedUpTime = timer;
        currentSlowDownTime = timer;
        currentKnockOutTime = timer;
    }

    #region StatusEffects
    #region SpeedUp
    /// <summary>
    /// Speed Up Functions
    /// </summary>
    [PunRPC]
    public void SpeedUp()
    {
        if (!isSpeeding)
        {
            currentSpeedUpTime = timer;
            StartCoroutine(Speeding());
        }
        else
        {
            currentSpeedUpTime = timer;
        }
    }

    IEnumerator Speeding()
    {
        // Double Up Movement Speed
        playerMovement.CurrentMoveSpeed = playerMovement.Speed * 2;
        SpeedClamp();
        isSpeeding = true;
        speedUpEffect.SetActive(true);

        // Power-Up Duration
        while (currentSpeedUpTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            currentSpeedUpTime--;
        }

        // Return to Normal
        playerMovement.CurrentMoveSpeed /= 2;
        SpeedClamp();
        isSpeeding = false;
        speedUpEffect.SetActive(false);
    }
    #endregion

    #region SlowDown
    /// <summary>
    /// Slow Down Functions
    /// </summary>
    [PunRPC]
    public void SlowDown()
    {
        if (!isSlowing)
        {
            currentSlowDownTime = timer;
            StartCoroutine(Slowing());
        }
        else
        {
            currentSlowDownTime = timer;
        }
    }

    IEnumerator Slowing()
    {
        // Slow Down Movement
        playerMovement.CurrentMoveSpeed = playerMovement.Speed / 2;
        SpeedClamp();
        isSlowing = true;
        slowDownEffect.SetActive(true);

        // Power-Up Duration
        while (currentSlowDownTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            currentSlowDownTime--;
        }

        // Return to Normal
        playerMovement.CurrentMoveSpeed *= 2;
        SpeedClamp();
        isSlowing = false;
        slowDownEffect.SetActive(false);
    }
    #endregion

    #region KnockOut
    /// <summary>
    /// Knock Out Functions
    /// </summary>
    [PunRPC]
    public void KnockOut()
    {
        if (!canKill)
        {
            currentKnockOutTime = timer;
            StartCoroutine(Knocking());
        }
        else
        {
            currentKnockOutTime = timer;
        }
    }

    IEnumerator Knocking()
    {
        // Player Can Kill while Effect is Active
        canKill = true;
        knockOutEffect.SetActive(true);

        // Power-Up Duration
        while (currentKnockOutTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            currentKnockOutTime--;
        }

        // Return to Normal
        canKill = false;
        knockOutEffect.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && canKill && collider.GetComponent<Health>().IsAlive)
        {
            // Knock Out Enemy Player
            collider.GetComponent<PhotonView>().RPC("OnDeath", RpcTarget.AllBuffered);
            currentKnockOutTime = 0;
            canKill = false;
            photonView.RPC("HasKnockedOut", RpcTarget.AllBuffered);
        }
    }
    #endregion

    /// <summary>
    /// Player Movement Speed Clamp
    /// </summary>
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

    /// <summary>
    /// Disable KO Effect after hitting another player
    /// </summary>
    [PunRPC]
    void HasKnockedOut()
    {
        knockOutEffect.SetActive(false);
    }
    #endregion

    #region PlayerStanding
    /// <summary>
    /// Converts Integer to Ordinal Number
    /// </summary>
    /// <param name="number"></param>
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

    /// <summary>
    /// Shows Ranking of the Player
    /// </summary>
    public void ShowPlayerRank()
    {
        TextMeshProUGUI playerStandingText = GameManager.Instance.PlayerStanding;

        if (photonView.IsMine)
        {
            playerStandingText.text = "YOU REACHED " + OrdinalPlace + " PLACE.";
        }
    }

    /// <summary>
    /// Add Player To Score List
    /// </summary>
    public void AddToScoreList()
    {
        ScoreManager.Instance.Players.Add(gameObject);
    }
    #endregion
}
