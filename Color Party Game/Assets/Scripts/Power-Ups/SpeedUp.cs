using UnityEngine;
using Photon.Pun;

public class SpeedUp : PowerUps
{
    public override void TakeEffect(Collider2D collider)
    {
        collider.GetComponent<PhotonView>().RPC("SpeedUp", RpcTarget.AllBuffered);
    }
}
