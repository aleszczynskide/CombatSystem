 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player20 : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    private int JumpForce = 8;
    private float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    private float mouseSensitivity = 60f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Mouse Movement
        Vector3 moveDirection = Vector3.zero;
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * mouseSensitivity * Time.deltaTime;
        float newRotationY = transform.eulerAngles.y + mouseX;
        transform.rotation = Quaternion.Euler(0, newRotationY, 0);

        // Movement Input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        moveDirection += transform.right * horizontalInput * moveSpeed;
        moveDirection += transform.forward * verticalInput * moveSpeed;

        rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
        // Mouse Movement

        if (horizontalInput != 0 || verticalInput != 0)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        anim.SetFloat("VelocityY", verticalInput);
        anim.SetFloat("VelocityX", horizontalInput);
    }
}

