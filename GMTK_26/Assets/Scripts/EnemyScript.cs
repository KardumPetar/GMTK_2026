using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed = 1;
    private Rigidbody2D rb;
    public bool turnToPlayer = true;
    [SerializeField] GameObject rotateToPlayer;
    public int rotSpeed = 10;
    private Vector3 newDir;

    public Vector2 direction = Vector2.zero;
    private bool _isFacingRight = false;
    //private bool _attackPlayer = false;

    private void Start() {
        rb  = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {

            direction  = collision.transform.parent.position - transform.position;
            direction = direction.normalized;

            if(turnToPlayer == true) { 
                if (direction.x > 0) {
                    //transform.Rotate(0f,-180f, 0f);
                    Turn(true);
                }
                else {
                    Turn(false);
                }
                    Attack();
            }
            if (rotateToPlayer != null) {
                //newDir = Vector3.RotateTowards(rotateToPlayer.transform.position, direction, rotSpeed* Time.deltaTime, 0.0f);
                rotateToPlayer.transform.rotation = Quaternion.LookRotation(direction);
            }
            //_attackPlayer = true;
        }
    }
    private void Turn(bool turnRight) {
        if (turnRight && !_isFacingRight) {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (!turnRight && _isFacingRight) {

            _isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(direction.x, direction.y, 0));
    }
    public virtual void Attack() {
        rb.velocity = new Vector2(Mathf.Sign(direction.x) * speed * Time.deltaTime, 0);
    }
}
