using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 15f;
    public float speedVertical = 15f;
    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void HandleMovement()
    {
        // vertical movement
        bool flyUp = Input.GetKey(KeyCode.Space);
        bool flyDown = Input.GetKey(KeyCode.LeftControl);
        var y = 0.0f;

        if (flyUp)
        {
            y = speed * Time.deltaTime;
        }
        else if (flyDown)
        {
            y = -1 * speed * Time.deltaTime;
        }

        // horizontal movement
        var x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        var z = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

        // speedup
        bool speedUp = Input.GetKey(KeyCode.LeftShift);
        if (speedUp)
        {
            x *= 2; z *= 2;
        }


        controller.Move(transform.right * x + transform.up * y + transform.forward * z);
    }

    void Update()
    {
        HandleMovement();
    }
}
