using Photon.Pun;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviourPun
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Transform _fireOffset;
    [SerializeField] private float _timeBetweenShots;

    private bool _fireContinuously;
    private bool _fireSingle;
    private float _lastFireTime;

    void Update()
    {
        if (_fireContinuously || _fireSingle)
        {
            float timeSinceLastFire = Time.time - _lastFireTime;

            if (timeSinceLastFire >= _timeBetweenShots)
            {
                Shoot();

                _lastFireTime = Time.time;
                _fireSingle = false;
            }
        }
    }

    public void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bullet"), _fireOffset.position, Quaternion.identity);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.velocity = _bulletSpeed * transform.up;
    }

    private void OnFire(InputValue inputValue)
    {
        _fireContinuously = inputValue.isPressed;

        if (inputValue.isPressed)
        {
            _fireSingle = true;
        }
    }
}
