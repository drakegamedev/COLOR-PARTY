using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class TimerManager : PunRaiseEvents
{
    public static TimerManager Instance;
    
    [Range(0, 59)] public int Minutes;
    [Range(0, 59)] public int Seconds;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI CountdownText;
    public ParticleSystem ArenaAesthetic;
    public float CountdownTimer;
    public float SetLastMinute;

    private float currentCountdownTime;
    private float currentTime;
    private bool isActive;
    private bool isLastMinute;
    private TimeSpan timer;

    public override void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public override void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public override void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventsCode.InitCountdownEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            StartCoroutine(InitiateCountdown());
        }
        else if (photonEvent.Code == (byte)RaiseEventsCode.TimerEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            StartCoroutine(Timer());
        }
        else if (photonEvent.Code == (byte)RaiseEventsCode.LastMinuteEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            StartCoroutine(LastMinute());
        }
        else if (photonEvent.Code == (byte)RaiseEventsCode.TimeOverEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            StartCoroutine(TimeOver());
        }
    }

    public override void SetRaiseEvent()
    {
        // event data
        object[] data = new object[] {  };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOption = new SendOptions
        {
            Reliability = false
        };

        // Call RaiseEvents
        if (currentCountdownTime >= 0 && !isActive)
        {
            PhotonNetwork.RaiseEvent((byte)RaiseEventsCode.InitCountdownEventCode, data, raiseEventOptions, sendOption);
        }
        else if (currentTime > 0 && isActive)
        {
            PhotonNetwork.RaiseEvent((byte)RaiseEventsCode.TimerEventCode, data, raiseEventOptions, sendOption);
        }
        else
        {
            PhotonNetwork.RaiseEvent((byte)RaiseEventsCode.TimeOverEventCode, data, raiseEventOptions, sendOption);
        }

        // Last Minute
        if (currentTime <= SetLastMinute && !isLastMinute)
        {
            PhotonNetwork.RaiseEvent((byte)RaiseEventsCode.LastMinuteEventCode, data, raiseEventOptions, sendOption);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentCountdownTime = CountdownTimer;
        currentTime = (Minutes * 60) + Seconds;
        
        isActive = false;
        isLastMinute = false;

        CallRaiseEvent();
    }

    // Start Countdown
    IEnumerator InitiateCountdown()
    {
        currentCountdownTime--;
        
        yield return new WaitForSeconds(1f);

        CountdownText.text = currentCountdownTime.ToString("0");

        // Go!
        if (currentCountdownTime <= 0f)
        {
            Debug.Log("Go!!!");
            CountdownText.text = "GO!";
            yield return new WaitForSeconds(1f);
            
            CountdownText.text = "";
            isActive = true;

            CallRaiseEvent();
            yield break;
        }

        CallRaiseEvent();
    }

    // Timer
    IEnumerator Timer()
    {
        currentTime--;
        timer = TimeSpan.FromSeconds(currentTime);
        TimerText.text = currentTime.ToString("0");

        if (currentTime > 0f && currentTime <= 10f)
        {
            CountdownText.text = timer.Seconds.ToString("0");
        }
        
        TimerText.text = timer.Minutes.ToString("00") + ":" + timer.Seconds.ToString("00");

        // Time is up
        if (currentTime <= 0)
        {
            CallRaiseEvent();
            yield break;
        }

        yield return new WaitForSeconds(1f);

        CallRaiseEvent();
    }

    IEnumerator LastMinute()
    {
        CountdownText.text = "LAST 2 MINUTES!";

        yield return new WaitForSeconds(2f);

        var main = ArenaAesthetic.main;

        CountdownText.text = "";
        main.simulationSpeed = 3f;
        isLastMinute = true;
    }

    IEnumerator TimeOver()
    {
        Debug.Log("Time's Up!");

        // Declare Time Over
        CountdownText.text = "Time's Up!";
        isActive = false;

        yield return new WaitForSeconds(2.0f);

        CountdownText.text = "";
        Debug.Log("Declare Winner!");
    }

    void CallRaiseEvent()
    {
        if (photonView.IsMine)
        {
            SetRaiseEvent();
        }
    }
}
