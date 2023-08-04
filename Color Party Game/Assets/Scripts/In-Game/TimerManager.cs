using System.Collections;
using UnityEngine;
using TMPro;
using System;
using Photon.Pun;

public class TimerManager : PunRaiseEvents
{
    [Header("Set Timer")]
    [Range(0, 59)] [SerializeField] private int minutes;                      // Set Number of Minutes
    [Range(0, 59)] [SerializeField] private int seconds;                      // Set Number of Seconds

    [Header("References")]
    [SerializeField] private TextMeshProUGUI timerText;                       // UI Timer Text
    [SerializeField] private TextMeshProUGUI countdownText;                   // UI Countdown Text
    
    [Header("Set Countdown and Last Minute Mechanic")]
    [SerializeField] private float countdownTimer;                            // Set Timer for Initial Countdown
    [SerializeField] private float setLastMinute;                             // Set Time Portion where Last Minute will be announced

    // Private Variables
    private float gameTime;
    private float currentCountdownTime;
    private float currentTime;
    private bool isLastMinute;
    private TimeSpan timer;

    public override void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public override void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        EventManager.Instance.EndGame -= TimeUp;
    }
  
    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        currentCountdownTime = countdownTimer;

        countdownText = GameManager.Instance.CountdownText;
        timerText = GameManager.Instance.TimerText;

        EventManager.Instance.EndGame += TimeUp;

        gameTime = (minutes * 60) + seconds;         // Add all timer inputs for Minutes and Seconds
        currentTime = gameTime;                      // Set Current Time
        isLastMinute = false;                        // Last Minute Phase Deactivated

        StartCoroutine(InitiateCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        // Start Timer when countdown is finished
        if (GameManager.Instance.GameState == GameManager.GameStates.PLAYING)
        {
            currentTime -= Time.deltaTime;

            // Convert currentTime float to Minutes/Seconds Form
            timer = TimeSpan.FromSeconds(currentTime);

            // Print Timer in Minutes/Seconds Form
            timerText.text = timer.Minutes.ToString("00") + ":" + timer.Seconds.ToString("00");

            // Final Countdown of 10 Seconds
            if (Mathf.Floor(currentTime) <= 10f)
            {
                countdownText.text = timer.Seconds.ToString("0");
            }

            // Call Last Minute, then intensify atmosphere
            if (Mathf.Floor(currentTime) <= 120 && !isLastMinute)
            {
                StartCoroutine(LastMinute());
                Debug.Log("LAST 2 MINUTES");
            }
            // Time's Up
            // Call TimeOver Function
            else if (Mathf.Floor(currentTime) <= 0f)
            {
                GameManager.Instance.GameState = GameManager.GameStates.GAME_OVER;
                StartCoroutine(TimeOver());
            }
        }
    }

    /// <summary>
    /// Start Countdown
    /// </summary>
    /// <returns></returns>
    IEnumerator InitiateCountdown()
    {
        // 1-second delay at Startup
        if (currentCountdownTime == countdownTimer)
        {
            yield return new WaitForSeconds(1f);
        }

        while (currentCountdownTime > 0f)
        {
            // Visual indication of Countdown
            countdownText.text = currentCountdownTime.ToString("0");
            yield return new WaitForSeconds(1f);

            // Decrement currentCountdownTime variable
            currentCountdownTime--;
        }

        // Game Officially Starts
        // Proceed to Timer Function
        countdownText.text = "GO!";

        yield return new WaitForSeconds(1f);

        GameManager.Instance.GameState = GameManager.GameStates.PLAYING;
        EventManager.Instance.InitiateGame.Invoke();

        // Show Score Panel while maintaining TimePanel
        PanelManager.Instance.ActivatePanel("score-panel");
        GameManager.Instance.TimePanel.SetActive(true);

        // Play BGM
        AudioManager.Instance.Play("game-bgm");

        countdownText.text = "";;
    }

    /// <summary>
    /// Announces Last Minute, Intensify Game Atmosphere
    /// </summary>
    /// <returns></returns>
    IEnumerator LastMinute()
    {
        // Announcement Text
        countdownText.text = "LAST 2 MINUTES!";
        isLastMinute = true;

        // Stop BGM and Change the Pitch
        AudioManager.Instance.Stop("game-bgm");
        AudioManager.Instance.ModifyPitch("game-bgm", 1.1f);

        yield return new WaitForSeconds(2f);

        // Play BGM with Higher Pitch
        AudioManager.Instance.Play("game-bgm");

        countdownText.text = "";

        // Intensity Atmosphere at Last Minute
        EventManager.Instance.Intensify.Invoke();
    }

    /// <summary>
    /// Called when Timer reaches 0
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeOver()
    {
        Debug.Log("Time's Up!");

        // Declare Time Over
        countdownText.text = "Time's Up!";

        EventManager.Instance.EndGame.Invoke();

        yield return new WaitForSeconds(2.0f);

        countdownText.text = "";

        PanelManager.Instance.ActivatePanel("results-panel");
        ScoreManager.Instance.SortScore();
        Debug.Log("Declare Winner!");
    }

    /// <summary>
    /// Time's Up
    /// </summary>
    void TimeUp()
    {
        currentTime = 0;
        timer = TimeSpan.FromSeconds(currentTime);
        timerText.text = timer.Minutes.ToString("00") + ":" + timer.Seconds.ToString("00");
    }
}