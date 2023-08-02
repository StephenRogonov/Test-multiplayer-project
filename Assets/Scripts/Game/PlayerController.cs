using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _screenBorder;
    [SerializeField] private GameObject _ui;
    [SerializeField] private GameObject _playerPointer;
    [SerializeField] private SpriteRenderer _bodySpriteRenderer;
    [SerializeField] private ParticleSystem _deathParticleSystem;

    private Rigidbody2D _rb;
    private PhotonView _photonView;
    private Camera _camera;
    private PlayerInput _input;
    private EndGame _endGameComponent;
    private Vector2 _movementInput;
    private Vector2 _smoothMovementInput;
    private Vector2 _smoothMovementInputVelocity;    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _photonView = GetComponent<PhotonView>();
        _endGameComponent = GetComponent<EndGame>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
    }

    private void Start()
    {
        if (!_photonView.IsMine)
        {
            Destroy(_input);
            Destroy(_rb);
            Destroy(_ui);
            Destroy(_playerPointer);
            Destroy(_endGameComponent);
        }

        Destroy(_playerPointer, 1.5f);

        var particleMainModule = _deathParticleSystem.main;
        particleMainModule.startColor = GameVars.GetColor(_photonView.Owner.ActorNumber);

        _bodySpriteRenderer.color = GameVars.GetColor(_photonView.Owner.ActorNumber);
    }

    private void FixedUpdate()
    {
        if (!_photonView.IsMine)
        {
            return;
        }

        SetPlayerVelocity();
        RotateInDirectionOfInput();
    }

    private void SetPlayerVelocity()
    {
        _smoothMovementInput = Vector2.SmoothDamp(_smoothMovementInput, _movementInput, ref _smoothMovementInputVelocity, 0.1f);
        _rb.velocity = _smoothMovementInput * _movementSpeed;

        PreventPlayerGoingOffScreen();
    }

    private void RotateInDirectionOfInput()
    {
        if (_movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _smoothMovementInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            _rb.MoveRotation(rotation);
        }
    }

    private void PreventPlayerGoingOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if ((screenPosition.x < _screenBorder && _rb.velocity.x < 0) ||
            (screenPosition.x > _camera.pixelWidth - _screenBorder && _rb.velocity.x > 0))
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }

        if ((screenPosition.y < _screenBorder && _rb.velocity.y < 0) ||
            (screenPosition.y > _camera.pixelHeight - _screenBorder && _rb.velocity.y > 0))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
        }
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

    public void PlayerDeath()
    {
        _photonView.RPC(nameof(RPC_PlayerDeath), RpcTarget.All);
        StartCoroutine(Leave());
    }

    [PunRPC]
    public void RPC_PlayerDeath()
    {
        _deathParticleSystem.Play();
        Destroy(gameObject.GetComponent<CapsuleCollider2D>());
        Destroy(_bodySpriteRenderer);
        Destroy(_input);
    }

    IEnumerator Leave()
    {
        yield return new WaitForSeconds(_deathParticleSystem.main.startLifetimeMultiplier);
        PhotonNetwork.LeaveRoom();
    }
}
