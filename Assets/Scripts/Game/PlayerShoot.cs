using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviourPun
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Transform[] _fireOffsets;
    [SerializeField] private float _timeBetweenShots;

    private PhotonView _photonView;
    private bool _fireContinuously;
    private bool _fireSingle;
    private float _lastFireTime;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }

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
        _photonView.RPC(nameof(RPC_Shoot), RpcTarget.All);
    }

    [PunRPC]
    public void RPC_Shoot()
    {
        foreach (Transform t in _fireOffsets)
        {
            GameObject bullet = Instantiate(_bulletPrefab, t.position, t.rotation);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = _bulletSpeed * transform.up;
        }
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
