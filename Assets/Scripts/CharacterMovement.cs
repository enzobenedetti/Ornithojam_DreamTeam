using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float holdingSpeed = 6f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float holdingStrengh;
    private float _maxFallSpeed = -5f;

    private float _velocityX;
    private float _velocityY;
    private float _velocityZ;

    private GameObject _cubeHold;
    private bool _isHolding;

    private bool InPause;
    
    void Awake()
    {
        if (!controller) controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InPause) return;
        _velocityY -= gravity * Time.deltaTime;
        if (_velocityY < _maxFallSpeed)
            _velocityY = _maxFallSpeed;

        if (_isHolding && _cubeHold)
        {
            float holdbuffer = Vector3.Distance(transform.forward.normalized,
                new Vector3(_velocityX, 0f, _velocityZ)) * 0.5f + 1f;
            
            _cubeHold.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3
                (_velocityX, 0f, _velocityZ).normalized * (holdingStrengh + holdbuffer), transform.position);
            _cubeHold.GetComponent<Rigidbody>().angularVelocity /= 1.5f;
        }

        controller.Move(new Vector3(_velocityX, _velocityY, _velocityZ)
                        * Time.deltaTime * (_isHolding ? holdingSpeed : speed));
        if (!_isHolding || !_cubeHold)
            transform.LookAt(transform.position + new Vector3(_velocityX, 0f, _velocityZ));
        else
        {
            transform.LookAt(_cubeHold.transform.position);
        }
    }

    public void OnMove(InputValue value)
    {
        Vector2 movement = value.Get<Vector2>();
        _velocityX = movement.x;
        _velocityZ = movement.y;
    }

    public void OnJump()
    {
        if (controller.isGrounded && !_isHolding)
        {
            _velocityY = Mathf.Sqrt(jumpForce);
        }
    }

    public void OnAction()
    {
        _isHolding = !_isHolding;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Movable") || _cubeHold) return;
        _cubeHold = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_cubeHold) return;
        if (_cubeHold == other.gameObject)
        {
            _cubeHold.transform.parent = null;
            _cubeHold = null;
        }
    }
}
