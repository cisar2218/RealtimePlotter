using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera followingCamera;
    [SerializeField] private float speed;

    private float horizontalInput;
    private float verticalInput;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        followingCamera.transform.position = transform.position;
        followingCamera.transform.rotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        var front = followingCamera.transform.forward * verticalInput * speed;
        var right = followingCamera.transform.right * horizontalInput * speed;
        var desiredPosition = this.transform.position + front + right;

        transform.position = Vector3.LerpUnclamped(transform.position, desiredPosition, Time.deltaTime);
    }
}
