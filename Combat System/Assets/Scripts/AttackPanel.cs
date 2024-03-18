using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackPanel : MonoBehaviour
{
    public GameObject Aphid;
    public GameObject Player;
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
            Destroy(gameObject);
            return;
        }
        else if (other.gameObject.CompareTag("Punch"))
        {
            Player.GetComponent<Player20>().anim.SetTrigger("NormalBlock");
            Player.GetComponent<Player20>().StaminaMinus();
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Player.GetComponent<PlayerMovement>().TakenDamage();
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.GetComponent<Aphid>().Hit == false)
            {
                other.GetComponent<Aphid>().OnHitReaction();
            }
        }
    }
}
