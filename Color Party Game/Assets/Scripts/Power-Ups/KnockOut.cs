using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KnockOut : PowerUps
{
    public override void TakeEffect(Collider2D collider)
    {
        collider.GetComponent<PhotonView>().RPC("KnockOut", RpcTarget.AllBuffered);
    }
}
