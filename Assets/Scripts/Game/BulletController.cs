using Photon.Pun;
using UnityEngine;

public class BulletController : MonoBehaviourPun
{
    [SerializeField] private float _destroyTime;
    [SerializeField] private float _damageValue;

    private void Start()
    {
        Destroy(gameObject, _destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if (collision.CompareTag("Damageable") && target.IsMine)
        {
            collision.GetComponent<HealthController>().TakeDamage(_damageValue);            
        }

        Destroy(gameObject);
    }
}
