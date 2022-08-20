using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class CountdownManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI CountdownText;
    private float Timer;

    // Start is called before the first frame update
    void Start()
    {
        CountdownText = GameManager.Instance.CountdownText;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            

            if (Timer > 0f)
            {
                Timer -= Time.deltaTime;
                photonView.RPC("SetTime", RpcTarget.AllBuffered, Timer);
            }
        }
        else
        {
            this.enabled = false;
        }
    }

    [PunRPC]
    public void SetTime(float timer)
    {
        CountdownText.text = timer.ToString("0");
    }
}
