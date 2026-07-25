using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy_Gun : EnemyScript
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject shootingPosition;

    [SerializeField] private float projectileSpeed;
    [SerializeField] private float cooldown_time;

    private float timeSinceLastShoot;
    private GameObject projectalsGO;

    public bool trow = false;
    private string attackAnimation = "shoot";
    private void Awake() {
        timeSinceLastShoot = 0;
        projectalsGO = GameObject.Find("Projectals");
        if (trow) {
            attackAnimation = "trow";
        }
     
    }

    
    public override void Attack() {
        
        animator.SetBool("isRuning", false);
        rb.velocity = Vector2.zero;
        if (timeSinceLastShoot>cooldown_time) {
            animator.SetTrigger(attackAnimation);
            GameObject newProjectal = Instantiate(projectile, projectalsGO.transform);
            newProjectal.transform.position = shootingPosition.transform.position;
            newProjectal.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;

            timeSinceLastShoot = 0;
        }
        timeSinceLastShoot += Time.deltaTime;
    }
    private void OnDestroy() {
        if(projectalsGO != null) {
            GameObject obj = Instantiate(animator.gameObject, projectalsGO.transform);
            obj.transform.position = transform.position;
            Animator objAnimator = obj.GetComponent<Animator>();
            objAnimator.enabled = true;
            objAnimator.SetTrigger("die");
        }
    }
}
