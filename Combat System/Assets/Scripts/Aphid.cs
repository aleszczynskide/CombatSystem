using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Aphid : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private Collider AphidCollider;
    private int AttackCount;
    [SerializeField] private GameObject PunchPointPrefab;
    [SerializeField] private GameObject PunchPointPosition;
    public bool ActivePunchPoint = false;
    public GameObject PunchPointHolder;
    public GameObject Player;
    private bool Follow = true;
    public bool Hit;
    public int AphidLife = 10;
    public bool Counter;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        AphidCollider = GetComponent<Collider>();
    }

    public void Update()
    {

     
        if (Follow)
        {
            FollowPlayer();
        }

        if (AttackCount == 30)
        {
            Follow = false;
            Hit = true;
            AttackCount = 0;
            animator.SetTrigger("Punch");
        }
    }
    public void Count()
    {
        if (Counter)
        {
            AttackCount++;
        }
        
    }
    public void DestroyPunchPoint()
    {
        Destroy(PunchPointHolder);
        PunchPointHolder = null;
        ActivePunchPoint = false;
        Hit = false;
    }
    public void AttackPointActive()
    {
        if (!ActivePunchPoint)
        {
            ActivePunchPoint = true;
            GameObject PunchPoint = Instantiate(PunchPointPrefab);
            PunchPointHolder = PunchPoint;
            PunchPoint.transform.parent = this.transform;
            PunchPoint.transform.position = PunchPointPosition.transform.position;
            PunchPoint.GetComponent<AttackPanel>().Aphid = this.gameObject;
            PunchPoint.GetComponent<AttackPanel>().Player = Player;
        }
    }
    public void Stun()
    {
        Follow = false;
        animator.SetTrigger("Stun");
        DestroyPunchPoint();
    }
    public void FollowPlayer()
    {
        Follow = true;

        Vector3 PlayerVector = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
        Vector3 AphidVector = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        Vector3 toTarget = PlayerVector - AphidVector;
        transform.LookAt(new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z));
        transform.Translate(toTarget * 0.3f * Time.deltaTime, Space.World);
    }
    public void OnHitReaction()
    {
        animator.SetTrigger("Hit");
        AphidLife -= 2;
        if (AphidLife == 0)
        {
            animator.SetTrigger("Death");
            Counter = false;
            AttackCount = 0;
            animator.ResetTrigger("Punch");
            this.gameObject.tag = "Untagged";
            Follow = false;
            rb.isKinematic = false;
        }
       
    }
    public void ResetTimer()
    {
        
        int x = Random.Range(0, 1);
        if (x == 0)
        {
            animator.ResetTrigger("Punch");
            AttackCount = 0;
        }
    }
}
