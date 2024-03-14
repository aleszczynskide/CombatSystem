using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aphid : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private int AttackCount;
    [SerializeField] private GameObject PunchPointPrefab;
    [SerializeField] private GameObject PunchPointPosition;
    public bool ActivePunchPoint = false;
    public GameObject PunchPointHolder;
    public GameObject Player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        Vector3 toTarget = Player.transform.position - transform.position;
        transform.LookAt(Player.transform.position);
        transform.Translate(toTarget * 2 * Time.deltaTime, Space.World);
        if (AttackCount == 2)
        {
            AttackCount = 0;
            animator.SetTrigger("Punch");
        }
    }
    public void Count()
    {
        AttackCount++;
    }
    public void DestroyPunchPoint()
    {
        Destroy(PunchPointHolder);
        PunchPointHolder = null;
        ActivePunchPoint = false;
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
        }
    }
    public void Stun()
    {
        animator.SetTrigger("Stun");
       DestroyPunchPoint();
    }
}
