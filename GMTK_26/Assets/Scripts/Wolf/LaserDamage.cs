using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    [SerializeField] int laserDamage;
    [SerializeField] float repeatTime;


    private float timeSinceHit = 0;


    private void FixedUpdate()
    {
        timeSinceHit += Time.fixedDeltaTime;
    }


    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && timeSinceHit > repeatTime)
        {
            CountDown.ReduceTime(laserDamage);
            Debug.Log("hit");
            timeSinceHit = 0;
        }
    }
}
