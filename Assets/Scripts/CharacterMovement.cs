using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float gravity = 10f;
    private float _maxFallSpeed = -5f;

    private float _velocityX;
    private float _velocityY;
    private float _velocityZ;
    
    void Awake()
    {
        if (!controller) controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _velocityY -= gravity * Time.deltaTime;
        if (_velocityY < _maxFallSpeed)
            _velocityY = _maxFallSpeed;
        controller.Move(new Vector3(_velocityX, _velocityY, _velocityZ) 
                        * Time.deltaTime * speed);
        transform.LookAt(transform.position + new Vector3(_velocityX, 0f, _velocityZ));
    }

    public void OnMove(InputValue value)
    {
        Vector2 movement = value.Get<Vector2>();
        Debug.Log(value.Get<Vector2>());
        _velocityX = movement.x;
        _velocityZ = movement.y;
    }

    public void OnJump()
    {
        if (controller.isGrounded)
        {
            _velocityY = Mathf.Sqrt(jumpForce);
        }
    }

    public void OnAction()
    {
        
    }
}
