using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeConsumeable : MonoBehaviour
{
    [SerializeField] int timeAdd = 10;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            CountDown.clockTime += timeAdd;
            Destroy(gameObject);
        }
        
    }
}
