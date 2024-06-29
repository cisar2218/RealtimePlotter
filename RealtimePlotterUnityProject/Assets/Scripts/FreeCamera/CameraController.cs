using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public Transform dron;

    [SerializeField] private float speed;

    public float mouseSensitivity;
    private float xRotation = 0.0f;

    public bool isWatchingDron = false;
    public bool dronToBeFound = false;
    private bool _freeCamera = false;
    public bool freeCamera
    {
        get { return _freeCamera; }
        set
        {
            _freeCamera = value;
            OnCameraModeChanged?.Invoke(_freeCamera);

            if (_freeCamera)
            {
                Debug.Log("Free camera enabled");
            }
        }
    }

    public event Action<bool> OnCameraModeChanged;

    void Start()
    {
        OnCameraModeChanged += (active) =>
        {
            if (active)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        };
    }

    void Update()
    {

        // Toggle 
        if (Input.GetKeyDown(KeyCode.C))
        {
            freeCamera = !freeCamera;
            Debug.Log("camera mode changed to " + freeCamera);
        }

        if (dronToBeFound)
        {
            FindDron();
            dronToBeFound = false;
        }
    }

    private void LateUpdate()
    {
        if (isWatchingDron)
        {
            transform.LookAt(dron, worldUp: Vector3.up);
        }

        if (!dronToBeFound && freeCamera)
        {
            MoveCamera();
        }

        if (!dronToBeFound && !isWatchingDron && freeCamera)
        {
            // mouse rotation
            float rotationX = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            float rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0f);
        }
    }

    private void MoveCamera()
    {
        // movement in plane
        Vector3 horizontalMove = transform.right * Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        Vector3 verticalMove = transform.forward * Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;
        Vector3 movementUp = Vector3.up * speed * Time.deltaTime;

        Vector3 movement = horizontalMove + verticalMove;

        // speed up
        bool speedUp = Input.GetKey(KeyCode.LeftShift);
        if (speedUp)
        {
            movement *= 2;
        }

        // up/down
        bool flyUp = Input.GetKey(KeyCode.Space);
        bool flyDown = Input.GetKey(KeyCode.LeftControl);

        if (flyUp)
        {
            movement += movementUp;
        }
        else if (flyDown)
        {
            movement -= movementUp;
        }

        // apply movements
        transform.position += movement;
    }


    private void FindDron()
    {
        transform.position = dron.position + 10 * (new Vector3(1, 1, 1));
        Debug.Log("camera position:" + transform.position);
        transform.LookAt(dron, worldUp: Vector3.up);
    }
}
