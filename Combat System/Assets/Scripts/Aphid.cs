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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (AttackCount == 2)
        {
            AttackCount = 0;
            animator.SetTrigger("Punch");
            if (!ActivePunchPoint)
            {
                ActivePunchPoint = true;
                GameObject PunchPoint = Instantiate(PunchPointPrefab);
                PunchPointHolder = PunchPoint;
                PunchPoint.transform.parent = this.transform;
                PunchPoint.transform.position = PunchPointPosition.transform.position;
            }
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
}
