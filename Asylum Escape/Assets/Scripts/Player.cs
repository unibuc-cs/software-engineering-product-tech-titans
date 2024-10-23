using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded;

    [SerializeField]
    private float _movementSpeed = 3.8f;

    private float _jumpHeight = 0.45f;
    private float _gravityValue = -9.8f;

    private void Start()
    {
        _controller = gameObject.AddComponent<CharacterController>();
        transform.position = new Vector3(0f, 0.76f, -6.0f);
    }

    void Update()
    {
        HandlePlayerMovement();
    }

    private bool IsGrounded()
    {
        // Raycast slightly below the player to check for ground
        return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f);
    }

    void HandlePlayerMovement()
    {
        // _isGrounded = _controller.isGrounded;
        _isGrounded = IsGrounded();
        if (_isGrounded && _velocity.y < 0.1f)
        {
            _velocity.y = 0f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        // Movement direction relative to player's forward direction;
        Vector3 move_direction = transform.right * horizontal + transform.forward * vertical;

        // So diagonal movement is not faster;
        if (move_direction.magnitude > 1.0f)
        {
            move_direction.Normalize();
        }

        // Slow player down when jumping so he cannot bunnyhop
        float currentSpeed = _movementSpeed;
        if (!_isGrounded)
        {
            currentSpeed *= 0.65f;
        }

        _controller.Move(move_direction * currentSpeed * Time.deltaTime);


        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        }

        _velocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        // Controller.isGrounded has problems, sometimes you can jump sometimes u cannot , need to fix
    }

}