using UnityEngine;
using System.Collections;
using Unity.Burst.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    private int JumpForce = 8;
    private float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    private float mouseSensitivity = 60f;
    private Rigidbody rb;
    private Animator animator;
    private bool IsGrounded = true;
    RaycastHit Hit;
    [SerializeField] private GameObject PunchPointPosition;
    [SerializeField] private GameObject PunchPointPrefab;
    public bool ActivePunchPoint = false;
    public GameObject PunchPointHolder;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Debug.Log(moveSpeed);
        if (Physics.Raycast(transform.position, Vector3.down, out Hit, 0.1f))
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
            if (!ActivePunchPoint)
            {
                ActivePunchPoint = true;
                GameObject PunchPoint = Instantiate(PunchPointPrefab);
                PunchPointHolder = PunchPoint;
                PunchPoint.transform.parent = PunchPointPosition.transform;
                PunchPoint.transform.position = PunchPointPosition.transform.position;
                PunchPoint.GetComponent<PunchParryView>().Player = this.gameObject;
            }
           
        }
        else
        {
            animator.SetBool("Block", false);
            if (ActivePunchPoint)
            {
                ActivePunchPoint = false;
                Destroy(PunchPointHolder);
            }
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
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("LeftWalk", true);
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetMouseButton(1))
            {

            }
        }
        else
        {
            animator.SetBool("LeftWalk", false);
        }
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftShift) )
        {
            animator.SetBool("RightWalk", true);
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetMouseButton(1))
            {
                moveSpeed = 5f;
            }
        }
        else
        {
            animator.SetBool("RightWalk", false);
        }
        if (Input.GetMouseButton(0))
        {
            if (IsGrounded)
            {
                animator.SetBool("Punch", true);
                if (ActivePunchPoint)
                {
                    ActivePunchPoint = false;
                    Destroy(PunchPointHolder);
                }
            }
        }

    }
    public void EndBools()
    {
        animator.SetBool("Jump", false);
        animator.SetBool("Punch", false);
    }
    public void SpeedUp()
    {
        moveSpeed = 7f;
    }
    public void SpeedDown()
    {
        moveSpeed = 3f;
    }
    public void NormalizeSpeed()
    {
        moveSpeed = 5f;
    }
    public void StunAnimation()
    {
        animator.SetTrigger("Stun");
    }
}
