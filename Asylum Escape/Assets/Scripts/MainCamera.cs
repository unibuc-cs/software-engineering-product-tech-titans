using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float mouseSensitivityX = 100f;
    public float mouseSensitivityY = 80f;

    // public Transform orientation;

    [SerializeField]
    private Transform _playerBody;

    private float _xRotation = 0f;
    private float _yRotation = 0f;

    void Start()
    {
        // Locks the cursor to the center of the screen;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        HandleMouseMovement();
        UpdatePosition();
    }

    void HandleMouseMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivityY * Time.deltaTime;

        _yRotation += mouseX;

        // Rotate the player body horizontally based on mouse X
        _playerBody.Rotate(Vector3.up * mouseX);

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f); /// Can't break your neck :)

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        // orientation.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }

    void UpdatePosition()
    {
        transform.position = _playerBody.transform.Find("POVLocation").gameObject.transform.position;
    }
}
