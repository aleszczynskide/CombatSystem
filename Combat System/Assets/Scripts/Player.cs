using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    private float mouseSensitivity = 60f;
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
            moveDirection += transform.forward * moveSpeed;
            animator.SetBool("Walk", true);
            animator.SetBool("WalkBackward", false);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDirection += -transform.forward * (moveSpeed);
            animator.SetBool("Walk", false);
            animator.SetBool("WalkBackward", true);
        }
        else
        {
            animator.SetBool("Walk", false);
            animator.SetBool("WalkBackward", false);
        }

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * mouseSensitivity * Time.deltaTime;
        float newRotationY = transform.eulerAngles.y + mouseX;
        transform.rotation = Quaternion.Euler(0, newRotationY, 0);

        float horizontalInput = Input.GetAxis("Horizontal");
        moveDirection += transform.right * horizontalInput * moveSpeed;

        rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
        if (Input.GetMouseButton(1))
        {
            animator.SetBool("Block", true);
        }
        else
        {
            animator.SetBool("Block", false);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                moveSpeed = 10f;
            }
            animator.SetBool("Run", true);
            animator.SetBool("Block", false);
        }
        else
        {
            moveSpeed = 5f;
            animator.SetBool("Run", false);
        }
    }
}
