using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 15f;
    private CharacterController controller;
    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        var z = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

        controller.Move(transform.right * x + transform.forward * z);
    }
}
