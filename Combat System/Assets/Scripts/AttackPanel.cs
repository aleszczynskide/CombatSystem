using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPanel : MonoBehaviour
{
    public GameObject Aphid;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PunchParry"))
        {
            Aphid.GetComponent<Aphid>().Stun();
        }
    }
}
