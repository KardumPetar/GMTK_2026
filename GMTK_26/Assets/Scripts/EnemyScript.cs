using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed = 1;
    public Rigidbody2D rb;
    public bool turnToPlayer = true;
    [SerializeField] GameObject rotateToPlayer;
    public int rotSpeed = 10;
    private Vector3 newDir;

    public Vector2 direction = Vector2.zero;
    private bool _isFacingRight = false;
    public bool _seePlayer = false;
    public float maxSearchTime;
    public float timeSinceSawPlayer = 0;


    public Animator animator;

    public bool _isPatrolinig;
    public bool _toA;
    [SerializeField] GameObject pointA;
    [SerializeField] GameObject pointB;
    [SerializeField] bool freeMovementAllowed= false;
    
    
    private void Start() {
        rb  = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            direction  = collision.transform.parent.position - transform.position;
            direction = direction.normalized;

            if(turnToPlayer == true) { 
                if (direction.x > 0) {
                    Turn(true);
                }
                else {
                    Turn(false);
                }
            }
            if (rotateToPlayer != null) {
                //newDir = Vector3.RotateTowards(rotateToPlayer.transform.position, direction, rotSpeed* Time.deltaTime, 0.0f);
                rotateToPlayer.transform.rotation = Quaternion.LookRotation(direction);
            }
            _seePlayer = true;
            _isPatrolinig = false;
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
        animator.SetBool("isRuning", true);
        rb.velocity = new Vector2(Mathf.Sign(direction.x) * speed * Time.deltaTime, 0);
    }
    public virtual void Search() {
        animator.SetBool("isRuning", true);
        rb.velocity = new Vector2(Mathf.Sign(direction.x) * speed * Time.deltaTime, 0);
    }
    private void Update() {

        
        if (_isPatrolinig) {
            if (_toA) {
                direction = pointA.transform.position - transform.position;                
            }
            else {
                direction = pointB.transform.position - transform.position;
            }
            if (Mathf.Abs( direction.x) < 1) {
                _toA = !_toA;            
            }
            direction = direction.normalized;
            
            if (direction.x > 0) {
                Turn(true);
            }
            else {
                Turn(false);
            }

            if (freeMovementAllowed)
            {
                rb.velocity = direction * speed * Time.deltaTime;
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Sign(direction.x) * speed * Time.deltaTime, 0);
            }
            
            
            
            animator.SetBool("isRuning", true);
            timeSinceSawPlayer += Time.deltaTime;
        }
        else if (_seePlayer) {
            Attack();
            timeSinceSawPlayer = 0;
        }
        else {
            Search();
            timeSinceSawPlayer += Time.deltaTime;
        }

        if(timeSinceSawPlayer > maxSearchTime) {
            _isPatrolinig = true;
        }
        _seePlayer = false;
    }
}
