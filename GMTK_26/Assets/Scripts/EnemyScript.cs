using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed = 1;
    private Rigidbody2D rb;


    public Vector2 direction = Vector2.zero;
    //private bool _attackPlayer = false;

    private void Start() {
        rb  = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {

            direction  = collision.transform.parent.position - transform.position;
            direction = direction.normalized;

            Attack();

            //_attackPlayer = true;
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(direction.x, direction.y, 0));
    }
    public virtual void Attack() {
        rb.velocity = new Vector2(Mathf.Sign(direction.x) * speed * Time.deltaTime, 0);
    }
}
