using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchParryView : MonoBehaviour
{
    [SerializeField] private GameObject PunchPrefab;
    [HideInInspector] public GameObject Player;
    public void PunchPoint()
    {
        GameObject PunchPoint = Instantiate(PunchPrefab);
        PunchPoint.transform.position = this.transform.position;
        Player.GetComponent<PlayerMovement>().PunchPointHolder = PunchPoint;
        PunchPoint.transform.parent = Player.transform;
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.CompareTag("NormalAttack"))
        {
            Debug.Log("Dupa");
        }
    }
}
