using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CharacterMovement : MonoBehaviour
{
    public HUDCompoenent HUDCompoenent;
    public CharacterController controller;
    public ParticleSystem moveParticule;
    public GameObject jumpParticule;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float holdingSpeed = 6f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float holdingStrengh;

    [Header("Respwan Parameters")] 
    public float RespawnTime=2f;
    public SkinnedMeshRenderer MeshRenderer;
    public GameObject PrefabRespawnParticul;
    
    private float _maxFallSpeed = -2f;
    private float _coyoteTime = 0.1f;
    private float _timeBalise;
    private bool _justNotGrounded = true;

    private Animator _animator;

    private float _timerSound;
    public float frequencyStep = 0.3f;
    public List<AudioClip> Clips;

    private float _velocityX;
    private float _velocityY;
    private float _velocityZ;

    private RaycastHit _slopeHit;
    private RaycastHit _rayHit;
    private float _groundRayDistance = 6f;
    private bool _isSliding;
   

    private GameObject _cubeHold;
    private bool _isHolding = false;
    private bool _isRespawning;
    private float _respawnTimer;
    private Vector3 _deathpos;
    private Vector3 _respawnPos;

    private bool InPause;

    public AudioClip nootNoot;

    void Awake()
    {
        if (!controller) controller = GetComponent<CharacterController>();
        if (!_animator) _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        AudioManager.Instance.PlaySound(nootNoot, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (InPause) return;
        if (_isRespawning) {
            ManageRespawn();
            return;
        }

        Debug.Log("On Ground is " +controller.isGrounded);
        if (OnSteepSlope())
        {
            Debug.Log("OnSteepSlope");
            _isSliding = true;
            SteepSlopeMovement();
        }
        else
        {
            if (_isSliding)
            {
                if (controller.isGrounded)
                {
                    Debug.Log("hello");
                    Debug.DrawRay(transform.position + Vector3.up/2f, -transform.forward * _groundRayDistance, Color.blue);
                    Debug.DrawRay(transform.position, Vector3.down *_groundRayDistance, Color.yellow);
                    if (Physics.Raycast(transform.position + Vector3.up/2f, -transform.forward, out _rayHit, 
                            (controller.height / 2) + _groundRayDistance) &&
                        !Physics.Raycast(transform.position, Vector3.down, _groundRayDistance/(_groundRayDistance*2f)))
                    {
                        SteepSlopeMovement();
                    }
                    else
                    {
                        _velocityX = 0f;
                        _velocityY = -1f;
                        _velocityZ = 0f;
                        _isSliding = false;
                    }
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
            if (!_justNotGrounded)
            {
                _animator.SetBool("InAir", false);
                _justNotGrounded = true;
            }
        }

        if (!controller.isGrounded ||
            Vector2.Distance(new Vector2(_velocityX, _velocityZ), Vector2.zero) < 0.1f)
        {
            moveParticule.Stop();
            _animator.SetBool("Walking", false);
            _timerSound = frequencyStep;
        }
        else
        {
            _timerSound += Time.deltaTime;
            if (_timerSound >= frequencyStep) {
                AudioManager.Instance.PlaySound(Clips[Random.Range(0,Clips.Count)], 1f);
                _timerSound = 0;
            }
            
            if (!moveParticule.isPlaying)
            {
                moveParticule.Play();
                _animator.SetBool("Walking", true);
            }
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
        
        HUDCompoenent.PressZ( _velocityZ>0);
        HUDCompoenent.PressS( _velocityZ<0);
        HUDCompoenent.PressQ( _velocityX<0);
        HUDCompoenent.PressD( _velocityX>0);
    }

    public void OnJump(InputValue value)
    {
        HUDCompoenent.PressSpace(value.isPressed);
        if (!value.isPressed) return;
        if ((controller.isGrounded || Time.time <= _timeBalise + _coyoteTime) && !_isHolding)
        {
            _velocityY = Mathf.Sqrt(jumpForce);
            Instantiate(jumpParticule, transform.position, Quaternion.identity);
            _animator.SetBool("InAir", true);
        }
    }

    public void OnAction(InputValue value)
    {
        HUDCompoenent.PressGrabe(value.isPressed);
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
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _groundRayDistance))
        {
            Debug.DrawRay(transform.position, Vector3.down * _groundRayDistance, Color.red);
            float slopeAngle = Vector3.Angle(_slopeHit.normal, Vector3.up);

            Debug.Log(slopeAngle);
            if (slopeAngle > controller.slopeLimit) return true;
        }
        return false;
    }

    void SteepSlopeMovement()
    {
        Vector3 slopeDirection = Vector3.up - _slopeHit.normal * Vector3.Dot(Vector3.up, _slopeHit.normal);
        float slideSpeed = speed + 1f + Time.deltaTime;

        _velocityX = slopeDirection.x * -slideSpeed;
        _velocityZ = slopeDirection.z * -slideSpeed;
        _velocityY -= _slopeHit.point.y * slideSpeed;

    }

    public void DoARespawn(Vector3 respawnPoint) {
        /*
        controller.enabled = false;
        transform.position = respawnPoint;
        Debug.Log(respawnPoint);
        _velocityX = 0f;
        _velocityY = 0f;
        _velocityZ = 0f;
        controller.enabled = true;
        */
        
        _velocityX = 0f;
        _velocityY = 0f;
        _velocityZ = 0f;
        controller.enabled = false;
        _deathpos = transform.position;
        _respawnPos = respawnPoint;
        _respawnTimer = 0;
        MeshRenderer.enabled = false;
        _isRespawning = true;
        moveParticule.Stop();
        Instantiate(PrefabRespawnParticul, transform.position, quaternion.identity);
    }

    private void ManageRespawn()
    {
        float t = _respawnTimer / RespawnTime;
        transform.position = Vector3.Lerp(_deathpos, _respawnPos, t);
        _respawnTimer += Time.deltaTime;
        if (_respawnTimer >= RespawnTime) {
            Instantiate(PrefabRespawnParticul, transform.position, quaternion.identity);
            MeshRenderer.enabled = true;
            AudioManager.Instance.PlaySound(nootNoot, 1f);
            _isRespawning = false;
            controller.enabled = true;
        }
    }
}
