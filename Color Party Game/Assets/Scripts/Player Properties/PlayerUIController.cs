using UnityEngine;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    public GameObject PlayerScoreItem { get; private set; }
    public TextMeshProUGUI PlayerNameText;
    
    private PlayerSetup playerSetup;

    // Start is called before the first frame update
    void Start()
    {
        playerSetup = GetComponent<PlayerSetup>();
        PlayerScoreItem = ScoreManager.Instance.ScoreItems[playerSetup.PlayerNumber - 1];
    }
}
