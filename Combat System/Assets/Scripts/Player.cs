using UnityEngine;
using System.Collections;
using Unity.Burst.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.Assertions;

public class PlayerMovement : MonoBehaviour
{
    private int JumpForce = 8;
    private float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    private float mouseSensitivity = 60f;
    private Rigidbody rb;
    [HideInInspector] public Animator animator;
    private bool IsGrounded = true;
    RaycastHit Hit;
    [SerializeField] private GameObject PunchPointPosition;
    [SerializeField] private GameObject PunchPointPrefab,AttackPointPrefab;
    public bool ActivePunchPoint = false;
    public GameObject PunchPointHolder;
    public GameObject CameraPosition;

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
                moveSpeed = 8f;
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
                if (!Input.GetMouseButton(1))
                {
                    IsGrounded = false;
                    animator.SetBool("Jump", true);
                    rb.velocity = new Vector3(0, +JumpForce, 0);
                }
                else
                {
                    animator.SetBool("Jump", true);
                    if (Input.GetKey(KeyCode.D))
                    {
                        rb.velocity = transform.TransformDirection(Vector3.right) * 7;
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        rb.velocity = transform.TransformDirection(Vector3.left) * 7;
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        rb.velocity = transform.TransformDirection(Vector3.back) * 7;
                    }

                }
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
        if (Input.GetMouseButtonDown(0))
        {
            if (IsGrounded) 
            {
                animator.SetBool("Punch", true);
   
                Cursor.visible = false;
                if (ActivePunchPoint)
                {
                    ActivePunchPoint = false;
                    Destroy(PunchPointHolder);
                }
            }
        }
        if (Input.GetKey(KeyCode.C))
        {
            animator.SetTrigger("Slide");
        }
    }
    public void EndBools()
    {
        animator.SetBool("Jump", false);
        animator.SetBool("Punch", false);
        animator.ResetTrigger("Slide");
    }
    public void SpeedUp()
    {
       // moveSpeed = 7f;
    }
    public void SpeedDown()
    {
        //moveSpeed = 3f;
    }
    public void NormalizeSpeed()
    {
        moveSpeed = 5f;
    }
    public void StunAnimation()
    {
        animator.SetTrigger("Stun");
    }
    public void TakenDamage()
    {
        Debug.Log("Dosta�");
    }
    public void DestroyAttackPanel()
    {
        Destroy(PunchPointHolder);
    }
    public void SpawnAttackPanel()
    {
        if (PunchPointHolder != null)
        {
            Destroy(PunchPointHolder);
        }
        GameObject PunchPoint = Instantiate(AttackPointPrefab);
        PunchPointHolder = PunchPoint;
        PunchPoint.transform.parent = PunchPointPosition.transform;
        PunchPoint.transform.position = PunchPointPosition.transform.position;
        PunchPoint.GetComponent<AttackPanel>().Player = this.gameObject;
    }
    public void SpawnBlock()
    {
        if (PunchPointHolder != null)
        {
            Destroy(PunchPointHolder);
        }
        ActivePunchPoint = true;
        GameObject PunchPoint = Instantiate(PunchPointPrefab);
        PunchPointHolder = PunchPoint;
        PunchPoint.transform.parent = PunchPointPosition.transform;
        PunchPoint.transform.position = PunchPointPosition.transform.position;
        PunchPoint.GetComponent<PunchParryView>().Player = this.gameObject;
    }
}
