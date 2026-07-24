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
        if(collision.TryGetComponent<DestroyableObject>(out destroyableObject)){
            destroyableObject.Hurt(damage);
        }
    }
}
