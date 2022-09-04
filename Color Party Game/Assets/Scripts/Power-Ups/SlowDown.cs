using UnityEngine;
using Photon.Pun;

public class SlowDown : PowerUps
{
    public override void TakeEffect(Collider2D collider)
    {
        foreach (GameObject go in GameManager.Instance.PlayerGameObjects)
        {
            if (collider.gameObject != go)
            {
                PhotonView photonView = go.GetComponent<PhotonView>();
                photonView.RPC("SlowDown", RpcTarget.AllBuffered);
            }
        }
    }
}
