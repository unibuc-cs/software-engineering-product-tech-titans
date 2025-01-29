using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float mouseSensitivityX = 100f;
    public float mouseSensitivityY = 80f;

    // public Transform orientation;

    [SerializeField]
    private Transform _playerBody;

    [Header("Raycasting")]
    public LayerMask clickableLayer;

    private float _xRotation = 0f;
    private float _yRotation = 0f;

    private OptionsMenu opsmenu;
    private float oldAcc = 1f;
    private float initialx;
    private float initialy;


    void Start()
    {
        //Cursor.lockState = CursorLockMode.None; 
        //Cursor.visible = true; 
        ////// Locks the cursor to the center of the screen;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        initialx = mouseSensitivityX;
        initialy = mouseSensitivityY;
    }


    void Update()
    {
        //Oare va tanca fps-urile ?
        //if(oldAcc != opsmenu.mousAcceleration)
        //{
        //    mouseSensitivityY = initialy * opsmenu.mousAcceleration;
        //    mouseSensitivityX = initialx * opsmenu.mousAcceleration;
        //}


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
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f); /// Can't break your neck :) :)

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        // orientation.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }

    void UpdatePosition()
    {
        transform.position = _playerBody.transform.Find("POVLocation").gameObject.transform.position;
      
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = new Ray(transform.position, transform.forward); 

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, clickableLayer))
            {
                Debug.Log($"Hit object: {hit.collider.gameObject.name} at position: {hit.point}");

            }
            else
            {
                Debug.Log("No object hit");
            }
        }
    }
}
