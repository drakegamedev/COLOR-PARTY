using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    public GameObject PlayerScoreItem { get { return playerScoreItem; } set { playerScoreItem = value; } }
    public TextMeshProUGUI PlayerNameText;
    
    private GameObject playerScoreItem;
    private PlayerSetup playerSetup;

    // Start is called before the first frame update
    void Start()
    {
        playerSetup = GetComponent<PlayerSetup>();
        playerScoreItem = ScoreManager.Instance.ScoreItems[playerSetup.PlayerNumber - 1];
    }
}
