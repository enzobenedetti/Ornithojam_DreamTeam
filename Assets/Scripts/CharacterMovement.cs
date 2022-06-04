using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public ParticleSystem moveParticule;
    public GameObject jumpParticule;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float holdingSpeed = 6f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float holdingStrengh;
    private float _maxFallSpeed = -2f;
    private float _coyoteTime = 0.1f;
    private float _timeBalise;
    private bool _justNotGrounded = true;

    private float _velocityX;
    private float _velocityY;
    private float _velocityZ;

    private RaycastHit _slopeHit;
    private RaycastHit _rayHit;
    private float _groundRayDistance = 6f;
    private bool _isSliding;

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

        if (OnSteepSlope())
        {
            _isSliding = true;
            SteepSlopeMovement();
        }
        else
        {
            if (_isSliding)
            {
                if (controller.isGrounded)
                {
                    _velocityX = 0f;
                    _velocityY = -1f;
                    _velocityZ = 0f;
                    _isSliding = false;
                }
                else
                {
                    SteepSlopeMovement();
                    _velocityX /= 20f;
                    _velocityZ /= 20f;
                    _velocityY += _maxFallSpeed * Time.deltaTime;
                }
            }
        }
        
        if (!controller.isGrounded)
        {
            _velocityY -= gravity * Time.deltaTime;
            if (_justNotGrounded)
            {
                _timeBalise = Time.time;
                _justNotGrounded = false;
            }
        }
        else
        {
            if (!_justNotGrounded) _justNotGrounded = true;
        }

        if (!controller.isGrounded ||
            Vector2.Distance(new Vector2(_velocityX, _velocityZ), Vector2.zero) < 0.1f)
        {
            moveParticule.Stop();
        }
        else if (!moveParticule.isPlaying)
        {
            moveParticule.Play();
        }
        
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
        if ((controller.isGrounded || Time.time <= _timeBalise + _coyoteTime) && !_isHolding)
        {
            _velocityY = Mathf.Sqrt(jumpForce);
            Instantiate(jumpParticule, transform.position + Vector3.down/2f, Quaternion.identity);
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

    bool OnSteepSlope()
    {
        if (!controller.isGrounded) return false;
        if (Physics.SphereCast(transform.position, controller.radius, Vector3.down, out _slopeHit, 
                (controller.height / 2) + _groundRayDistance))
        {
            float slopeAngle = Vector3.Angle(_slopeHit.normal, Vector3.up);
            Debug.Log(slopeAngle);
            if (Physics.Raycast(transform.position, Vector3.down, out _rayHit,
                    (controller.height / 2) + _groundRayDistance))
            {
                if (Vector3.Dot(_rayHit.normal, _slopeHit.normal) < 0.9f &&
                    Vector3.Angle(_rayHit.normal, Vector3.up) <= controller.slopeLimit) return false;
            }
            else return false;

            if (slopeAngle > controller.slopeLimit && slopeAngle < 89f) return true;
        }
        return false;
    }

    void SteepSlopeMovement()
    {
        Vector3 slopeDirection = Vector3.up - _slopeHit.normal * Vector3.Dot(Vector3.up, _slopeHit.normal);
        float slideSpeed = speed + 2f + Time.deltaTime;

        
        _velocityX = slopeDirection.x * -slideSpeed;
        _velocityZ = slopeDirection.z * -slideSpeed;
        _velocityY -= _slopeHit.point.y;

    }
}
