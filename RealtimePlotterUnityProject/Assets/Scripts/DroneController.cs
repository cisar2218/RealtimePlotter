using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]
public class DroneController : MonoBehaviour
{
    public float speed = 15f;

    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    float GetVerticalMovement()
    {
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
        return y;
    }

    void Update()
    {
        // drop movement enabled
        bool dronEnabled = Input.GetKey(KeyCode.LeftAlt);
        if (!dronEnabled) { return; }

        // horizontal movement
        var x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        var z = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

        var y = GetVerticalMovement();

        controller.Move(transform.right * -x + /* transform.up * 0 + */ transform.up * z);
    }
}
