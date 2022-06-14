using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    public GameObject[] whiteWalls;

    // Timer
    private float currentTime;
    public int startMinutes;
    public TextMeshProUGUI currentTimeText;
    public bool timerActive;
    private bool lastMinute;

    public TextMeshProUGUI countdownTimeText;

    private TimeSpan time;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startMinutes * 60;
        lastMinute = false;

        time = TimeSpan.FromSeconds(currentTime);

        foreach (GameObject go in whiteWalls)
        {
            go.GetComponent<LerpColor>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive == true)
        {
            currentTime -= Time.deltaTime;
            time = TimeSpan.FromSeconds(currentTime);

            if (Mathf.Floor(currentTime) <= 120 && lastMinute == false)
            {
                StartCoroutine(LastMinute());

                // Modify Pitch of the BGM
                AudioManager.Instance.Stop("bgm");
                AudioManager.Instance.ModifyPitch("bgm", 1.25f);
                AudioManager.Instance.Play("bgm");
            }

            if (currentTime > 1f && Mathf.Floor(currentTime) <= 10f)
            {
                countdownTimeText.text = time.Seconds.ToString("0");
            }

            if (currentTime <= 0.9f)
            {
                timerActive = false;
                StartCoroutine(GameManager.Instance.TimeOver());
                Debug.Log("Time Over");

                foreach (GameObject go in GameManager.Instance.playerGO)
                {
                    go.GetComponent<PlayerSetup>().GetAnimator().enabled = false;
                    go.GetComponent<PlayerSetup>().GetPlayerMovement().enabled = false;
                }
            }

            currentTimeText.text = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
        }
    }

    // Get Current Time
    public float GetCurrentTime()
    {
        return currentTime;
    }

    // Indicate last 2 minutes
    IEnumerator LastMinute()
    {
        countdownTimeText.text = "LAST 2 MINUTES!";

        lastMinute = true;

        yield return new WaitForSeconds(1.5f);        

        countdownTimeText.text = "";

        foreach (GameObject go in whiteWalls)
        {
            go.GetComponent<LerpColor>().enabled = true;
        }
    }
}
