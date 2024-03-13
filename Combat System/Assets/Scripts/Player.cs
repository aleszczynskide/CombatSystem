using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += transform.forward;
            animator.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDirection += -transform.forward;
            animator.SetBool("Run", true);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);

        if (Input.GetMouseButton(1))
        {
            animator.SetBool("Block", true);
        }
        else
        {
            animator.SetBool("Block", false);
        }

        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }
}


