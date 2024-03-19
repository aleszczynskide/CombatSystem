using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player20 : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rb;
    private int JumpForce = 8;
    private float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    private float mouseSensitivity = 60f;
    [SerializeField] private GameObject PunchPointPosition;
    [SerializeField] private GameObject PunchPointPrefab, AttackPointPrefab;
    public bool ActivePunchPoint = false;
    public GameObject PunchPointHolder;
    private int Stamina = 10;
    public Image staminaImage;
    /*[HideInInspector]*/
    public int PunchCount;
    private bool PunchCounter = true;

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

        //Attack and blocking logic
        if (Input.GetMouseButton(1))
        {
            if (Stamina > 0)
            {
                anim.SetBool("Block", true);
            }
            else
            {
                anim.SetBool("Block", false);
                anim.SetBool("No", true);
            }

        }
        else
        {
            anim.SetBool("Block", false);
            if (ActivePunchPoint)
            {
                ActivePunchPoint = false;
                Destroy(PunchPointHolder);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (PunchCounter == true)
            {
                if (Stamina > 0)
                {
                    anim.SetBool("Punch", true);
                    if (PunchCount < 3)
                    {
                        PunchCount++;
                    }
                }
                else
                {
                    anim.SetBool("No", true);
                }
            }
        }
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
    public void StunAnimation()
    {
        anim.SetBool("Stun", true);
    }
    public void ResetBools()
    {
        anim.SetBool("Stun", false);
        anim.SetBool("NormalBlock", false);
        anim.SetBool("No", false);
        anim.SetBool("Punch", false);
        PunchCounter = true;
    }
    public void StaminaMinus()
    {
        Stamina -= 5;
        staminaImage.fillAmount = staminaImage.fillAmount - 0.5f;
    }
    public void StopCounting()
    {
        PunchCounter = false;
        PunchCount--;
    }
    public void CheckNextPunch()
    {
        if (PunchCount > 0)
        {
            anim.SetBool("Punch", true);
            PunchCount--;
        }
        else
        {
            anim.SetBool("Punch", false);
            PunchCounter = true;
            PunchCount = 0;
        }
    }
    public void TurnOnCounting()
    {
        PunchCounter = true;
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
    public void DestroyAttackPanel()
    {
        Destroy(PunchPointHolder);
    }
    public void UseStamina()
    {
        Stamina -= 1;
        staminaImage.fillAmount -= 0.1f;
    }
    public void AddStamina()
    {
        Stamina += 1;
        staminaImage.fillAmount += 0.1f;
    }
}

