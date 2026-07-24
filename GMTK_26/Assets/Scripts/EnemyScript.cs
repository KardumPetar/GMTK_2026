using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed = 1;
    private Vector2 direction = Vector2.zero;
    private Rigidbody2D rb;

    private void Start() {
        rb  = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            direction  = collision.transform.position - transform.position;
            rb.velocity =new Vector2( Mathf.Sign(direction.x)*speed*Time.deltaTime, 0);
        }
    }
}
