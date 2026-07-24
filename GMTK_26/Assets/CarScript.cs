using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        
    }
    private void Update() {
        rb.velocity = Vector2.left * speed * Time.deltaTime;
    }
}
