using UnityEngine;
using System.Collections;
using Unity.Burst.CompilerServices;
using JetBrains.Annotations;

public class PlayerMovement : MonoBehaviour
{
    private int JumpForce = 8;
    private float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    private float mouseSensitivity = 60f;
    private Rigidbody rb;
    private Animator animator;
    public bool IsGrounded = false;
    RaycastHit Hit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out Hit, 0.001f))
        {
            if ((Hit.collider.CompareTag("Ground")))
            {
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }
        }
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
        }
        else
        {
            moveSpeed = 5f;
            animator.SetBool("Run", false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded)
            {
                IsGrounded = false;
                animator.SetBool("Jump", true);
                rb.velocity = new Vector3(0, +JumpForce, 0);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("LeftWalk", true);
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = 3f;
            }
        }
        else
        {
            animator.SetBool("LeftWalk", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("RightWalk", true);
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = 3f;
            }
        }
        else
        {
            animator.SetBool("RightWalk", false);
        }
    }
    public void EndJump()
    {
        animator.SetBool("Jump", false);
    }
}
