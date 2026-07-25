using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    private Collider2D collider2d;
    public int damage;
    private DestroyableObject destroyableObject;
    private void Start() {
        collider2d = GetComponent<Collider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.isTrigger) { 
            destroyableObject = collision.GetComponentInParent<DestroyableObject>();
            if(destroyableObject!=null){
                destroyableObject.Hurt(damage);
            }
            else if(collision.CompareTag("Player")) {
                CountDown.ReduceTime(damage);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.collider.isTrigger) {
            destroyableObject = collision.collider.GetComponentInParent<DestroyableObject>();
            if (destroyableObject != null) {
                destroyableObject.Hurt(damage);
            }
            else if (collision.collider.CompareTag("Player")) {
                CountDown.ReduceTime(damage);
            }
        }
    }
}
