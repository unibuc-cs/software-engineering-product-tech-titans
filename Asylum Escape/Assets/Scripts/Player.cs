using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject EndScreen;
    private CharacterController _controller;
    private Vector3 _velocity;
    public bool _isGrounded;
    public int hp;
    public float hpTimer = 0;
    public float hpTimeThreshold = 20;

    [SerializeField]
    private float _movementSpeed = 3.8f;

    private float _jumpHeight = 0.45f;
    private float _gravityValue = -9.8f;

    private void Start()
    {
        hp = 2;
        _controller = gameObject.AddComponent<CharacterController>();
        transform.position = new Vector3(0f, 0.76f, -6.0f);
    }

    void Update()
    {
        if (hp == 1)
        {
            hpTimer += Time.deltaTime;
        }
        if (hpTimer >= hpTimeThreshold) {
            hp = 2;
            hpTimer = 0;
        }

        if (hp == 0)
        {
            //Debug.Log("END");
            StartCoroutine(FadeToEndScene());
        }
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

    public float getMovementSpeed()
    {
        return _movementSpeed; 
    }

    public void setMovementSpeed(float value)
    {
        _movementSpeed = value;
    }

    public Vector3 getVelocity()
    {
        return _controller.velocity; 
    }

    private IEnumerator FadeToEndScene()
    {
        // So the demon animates his attack before the player dies
        yield return new WaitForSeconds(0.5f);

        Image fadeImage = EndScreen.GetComponent<Image>();
        Color fadeColor = fadeImage.color;
        float elapsedTime = 0f;
        float fadeDuration = 1.5f;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        EndScreen.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            fadeColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            if (fadeColor.a > 0.9) fadeColor.a = 0.9f;
            fadeImage.color = fadeColor;
            yield return null;
        }
    }
}