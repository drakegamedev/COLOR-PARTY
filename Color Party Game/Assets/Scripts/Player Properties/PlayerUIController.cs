using UnityEngine;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    public GameObject PlayerScoreItem { get; private set; }
    public TextMeshProUGUI PlayerNameText;
    
    // Private Variables
    private PlayerSetup playerSetup;                                    // PlayerSetup Class Reference

    // Start is called before the first frame update
    void Start()
    {
        playerSetup = GetComponent<PlayerSetup>();

        // Get Score Item Based on Player Index
        PlayerScoreItem = ScoreManager.Instance.ScoreItems[playerSetup.PlayerNumber - 1];
    }
}
