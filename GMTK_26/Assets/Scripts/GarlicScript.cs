
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GarlicScript : MonoBehaviour
{
    private Collider2D collider2d;
    public int timePenalty;

    //private bool imune = false;
    private float timeSinceHit = 0;
    public float imunityTime = 1;
    private void Awake() {
        collider2d = GetComponent<Collider2D>();
    }

    private void FixedUpdate() {
        timeSinceHit += Time.fixedDeltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            CountDown.ReduceTime(timePenalty);
            timeSinceHit = 0;
        }
    }
}
