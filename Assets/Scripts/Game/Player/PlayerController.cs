using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _screenBorder;

    private Rigidbody2D _rb;
    private PhotonView _photonView;
    private Vector2 _movementInput;
    private Vector2 _smoothMovementInput;
    private Vector2 _smoothMovementInputVelocity;
    private Camera _camera;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _photonView = GetComponent<PhotonView>();
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
            Destroy(_rb);
        }
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

    private void RotateInDirectionOfInput()
    {
        if (_movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _smoothMovementInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            _rb.MoveRotation(rotation);
        }
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}
