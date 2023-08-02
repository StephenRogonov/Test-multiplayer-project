using Photon.Pun;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if (collision.CompareTag("Damageable") && target.IsMine)
        {
            collision.GetComponent<CoinCounter>().CoinCollect();
        }

        Destroy(gameObject);
    }
}
