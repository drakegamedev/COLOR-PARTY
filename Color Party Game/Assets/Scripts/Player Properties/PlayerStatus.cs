using System.Collections;
using UnityEngine;
using TMPro;
using Photon.Pun;

// Manages Status Effects, and Indicates Player Standing
public class PlayerStatus : MonoBehaviourPunCallbacks
{
    public int Place { get; set; }                              // Indicate Player Rank (e.g. 1,2,3, etc.)
    public string OrdinalPlace { get; private set; }            // Add Suffix to Player Rank (e.g. 1st, 2nd, 3rd)
    public float Timer;                                         // Status Effect Duration

    private PlayerMovement playerMovement;

    // Speed Up Variables
    public GameObject SpeedUpEffect;
    private bool isSpeeding;
    private float currentSpeedUpTime;

    // Slow Down Variables
    public GameObject SlowDownEffect;
    private bool isSlowing;
    private float currentSlowDownTime;

    // Knock Out Variables
    public GameObject KnockOutEffect;
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
        currentSpeedUpTime = Timer;
        currentSlowDownTime = Timer;
        currentKnockOutTime = Timer;
    }

    #region StatusEffects
    #region SpeedUp
    [PunRPC]
    // Speed Up Functions
    public void SpeedUp()
    {
        if (!isSpeeding)
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
        // Double Up Movement Speed
        playerMovement.CurrentMoveSpeed = playerMovement.Speed * 2;
        SpeedClamp();
        isSpeeding = true;
        SpeedUpEffect.SetActive(true);

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
        SpeedUpEffect.SetActive(false);
    }
    #endregion

    #region SlowDown
    // Slow Down Functions
    [PunRPC]
    public void SlowDown()
    {
        if (!isSlowing)
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
        // Slow Down Movement
        playerMovement.CurrentMoveSpeed = playerMovement.Speed / 2;
        SpeedClamp();
        isSlowing = true;
        SlowDownEffect.SetActive(true);

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
        // Player Can Kill while Effect is Active
        canKill = true;
        KnockOutEffect.SetActive(true);

        // Power-Up Duration
        while (currentKnockOutTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            currentKnockOutTime--;
        }

        // Return to Normal
        canKill = false;
        KnockOutEffect.SetActive(false);
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

    // Disable KO Effect after hitting another player
    [PunRPC]
    void HasKnockedOut()
    {
        KnockOutEffect.SetActive(false);
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
