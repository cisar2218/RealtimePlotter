using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform player;

    public float mouseSensitivity;
    private float xRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // MOUSE
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ARROWS
       

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
        player.Rotate(mouseX * Vector3.up);
        //transform.position = transform.forward * verticalInput * speed;
        //transform.rotation = transform.right * horizontalInput * speed;
    }

    //private void FixedUpdate()
    //{
    //    var front = followingCamera.transform.forward * verticalInput * speed;
    //    var right = followingCamera.transform.right * horizontalInput * speed;
    //    var desiredPosition = this.transform.position + front + right;

    //    transform.position = Vector3.LerpUnclamped(transform.position, desiredPosition, Time.deltaTime);
    //}
}
