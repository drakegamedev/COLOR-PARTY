using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerStatus : MonoBehaviourPunCallbacks
{
    public GameObject[] tiles;
    public GameObject[] statusEffects;
    public int playerScore;
    private int score;
    public string playerName;
    private Color playerColor;

    [SerializeField]
    private int playerActorNumber;

    public GameObject explosion;
    private int place;
    private string ordinalPlace;

    // Power-up booleans
    private bool canKill;

    public float r;
    public float g;
    public float b;

    // Start is called before the first frame update
    void Start()
    {
        playerName = photonView.Owner.NickName;
        playerScore = 0;
        playerColor = new Color(r, g, b);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // If player collides with a tile
        if (collider.tag == "Tile")
        {
            if (collider.GetComponent<SpriteRenderer>().color == new Color(r, g, b))
            {
                Debug.Log("Color is the same");
                return;
            }
            else if (collider.GetComponent<SpriteRenderer>().color == Color.white)
            {
                collider.GetComponent<SpriteRenderer>().color = new Color(r, g, b);

                if (photonView.IsMine)
                {
                    this.photonView.RPC("IncreaseScore", RpcTarget.AllBuffered);
                }

                collider.GetComponent<Tiles>().SetPlayerStep(gameObject);

                Debug.Log("New Color");
            }
            else
            {
                if (photonView.IsMine)
                {
                    this.photonView.RPC("IncreaseScore", RpcTarget.AllBuffered);
                }

                GameObject otherPlayer = collider.GetComponent<Tiles>().GetPlayerStep();

                if (otherPlayer.GetComponent<PhotonView>().IsMine)
                {
                    otherPlayer.GetComponent<PhotonView>().RPC("DecreaseScore", RpcTarget.AllBuffered);
                }

                collider.GetComponent<SpriteRenderer>().color = new Color(r, g, b);
                collider.GetComponent<Tiles>().SetPlayerStep(gameObject);
                Debug.Log("Replace Color");
            }
        }

        // If player collides with another player
        if (collider.tag == "Player" && canKill == true && !collider.gameObject.GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("You hit an enemy");
            photonView.RPC("KillOpposingPlayer", RpcTarget.AllBuffered);
            collider.gameObject.GetComponent<PhotonView>().RPC("PlayerKilled", RpcTarget.AllBuffered);

            if (photonView.IsMine)
            {
                AudioManager.Instance.Play("explode");
            }
        }

        // If player picks up an power-up
        if (collider.tag == "Power-Up")
        {
            PowerUps power = collider.GetComponent<PowerUps>();
            
            if (power.type == PowerUps.PowerUpType.SPEED_UP)
            {
                photonView.RPC("SpeedUp", RpcTarget.AllBuffered);
            }
            else if (power.type == PowerUps.PowerUpType.SLOW_DOWN)
            {
                photonView.RPC("SlowDown", RpcTarget.AllBuffered);
            }
            else if (power.type == PowerUps.PowerUpType.KNOCK_OUT)
            {
                photonView.RPC("KnockOut", RpcTarget.AllBuffered);
            }

            if (photonView.IsMine)
            {
                AudioManager.Instance.Play("powerUp");
            }
            
            Destroy(collider.gameObject);
        }
    }

    // Increase Player Score
    [PunRPC]
    public void IncreaseScore()
    {
        this.playerScore++;
        GameManager.Instance.playerScoreItems[playerActorNumber - 1].GetComponent<PlayerScoreItem>().scoreText.text = this.playerScore.ToString();
    }

    // Decrease Player Score
    [PunRPC]
    public void DecreaseScore()
    {
        this.playerScore--;

        if (this.playerScore < 0)
        {
            this.playerScore = 0;
        }

        GameManager.Instance.playerScoreItems[playerActorNumber - 1].GetComponent<PlayerScoreItem>().scoreText.text = this.playerScore.ToString();
    }

    [PunRPC]
    public void SpeedUp()
    {
        StartCoroutine(SpeedUpTimer());
    }

    [PunRPC]
    public void SlowDown()
    {
        foreach (GameObject go in GameManager.Instance.playerGO)
        {
            if (this.gameObject != go)
            {
                go.GetComponent<PlayerMovement>().speed = 1.5f;
                go.GetComponent<PlayerStatus>().statusEffects[1].SetActive(true);
                go.GetComponent<PlayerStatus>().StartCoroutine(SlowDownTimer());
            }
        }
    }

    [PunRPC]
    public void KnockOut()
    {
        canKill = true;
        statusEffects[2].SetActive(true);
  
        StartCoroutine(KnockOutTimer());
    }

    [PunRPC]
    public void KillOpposingPlayer()
    {
        canKill = false;
        statusEffects[2].SetActive(false);
        StopCoroutine(KnockOutTimer());

        // Spawn explosion
        GameObject explosionPrefab = Instantiate(explosion, this.transform.position, Quaternion.identity);

        // Destroy explosion
        Destroy(explosionPrefab, 1.0f);
    }

    // When player is killed
    [PunRPC]
    public void PlayerKilled()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(Respawn());
        }

        StartCoroutine(ChangeColor());
    }

    // Timer for the Speed Up Power-up
    IEnumerator SpeedUpTimer()
    {
        gameObject.GetComponent<PlayerMovement>().speed = 10;
        statusEffects[0].SetActive(true);

        yield return new WaitForSeconds(5f);
        
        gameObject.GetComponent<PlayerMovement>().speed = 5;
        statusEffects[0].SetActive(false);
    }

    // Timer for the Slow Down Power-up
    IEnumerator SlowDownTimer()
    {
        yield return new WaitForSeconds(5f);

        foreach (GameObject go in GameManager.Instance.playerGO)
        {
            if (this.gameObject != go)
            {
                go.GetComponent<PlayerMovement>().speed = 5;
                go.GetComponent<PlayerStatus>().statusEffects[1].SetActive(false);
            }
        }
    }

    IEnumerator KnockOutTimer()
    {
        yield return new WaitForSeconds(5f);

        canKill = false;
        statusEffects[2].SetActive(false);
    }

    IEnumerator Respawn()
    {
        GetComponent<PlayerMovement>().enabled = false;

        yield return new WaitForSeconds(2f);

        transform.position = GameManager.Instance.spawnPoints[playerActorNumber - 1].position;
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<SpriteRenderer>().color = new Color(r, g, b);
    }

    // Change opposing player's color when hit.
    IEnumerator ChangeColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        yield return new WaitForSeconds(2f);
        GetComponent<SpriteRenderer>().color = new Color(r, g, b);
    }

    public void ToOrdinal(int value)
    {
        // Default suffix.
        string suffix = "TH";

        // Get the last 2 digits.
        int lastTwoDigits = value % 100;

        // If the last 2 digits are 11, 12, or 13, use th. Otherwise:
        if (lastTwoDigits < 11 || lastTwoDigits > 13)
        {
            // Check the last digit.
            switch (lastTwoDigits % 10)
            {
                case 1:
                    suffix = "ST";
                    break;
                case 2:
                    suffix = "ND";
                    break;
                case 3:
                    suffix = "RD";
                    break;
            }
        }

        ordinalPlace = value + suffix;
    }

    public void ShowPlayerRank()
    {
        TextMeshProUGUI playerStandingText = GameManager.Instance.playerStanding;

        if (photonView.IsMine)
        {
            playerStandingText.text = "YOU REACHED " + ordinalPlace + " PLACE.";
        }
    }

    // Getters
    public int GetActorNumber()
    {
        return playerActorNumber;
    }

    public string GetOrdinalPlace()
    {
        return ordinalPlace;
    }

    // Setters
    public void SetPlace(int value)
    {
        place = value;
    }
}
