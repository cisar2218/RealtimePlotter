using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 15f;
    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        var x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        var z = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

        controller.Move(transform.right * x + transform.forward * z);
    }
}
