using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamage : MonoBehaviour
{
    [SerializeField] int AOE_Damage;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CountDown.ReduceTime(AOE_Damage);
            //Debug.Log("hit");
        }
    }
}
