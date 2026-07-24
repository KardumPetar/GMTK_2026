
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GarlicScript : MonoBehaviour
{
    private Collider2D collider2d;
    public int timePenalty;
    private Collider2D playerColider;

    //private bool imune = false;
    private float timeSinceHit = 0;
    public float imunityTime = 1;
    private void Awake() {
        collider2d = GetComponent<Collider2D>();
        
        playerColider = (Collider2D)FindFirstObjectByType(typeof(CapsuleCollider2D), FindObjectsInactive.Include);//.GetComponent<Collider2D>();
    }


    private void FixedUpdate() {
        if (collider2d.IsTouching(playerColider) && timeSinceHit>imunityTime) {
            CountDown.clockTime -= timePenalty;
            timeSinceHit = 0;
        }
        timeSinceHit += Time.fixedDeltaTime;
    }
    private void OnTriggerEnter(Collider other) {
        
    }
}
