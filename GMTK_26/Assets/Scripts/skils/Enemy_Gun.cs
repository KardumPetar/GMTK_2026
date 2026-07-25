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


    private void Awake() {
        timeSinceLastShoot = 0;
        projectalsGO = GameObject.Find("Projectals");
    }

    
    public override void Attack() {
        
        if (timeSinceLastShoot>cooldown_time) {

            GameObject newProjectal = Instantiate(projectile, projectalsGO.transform);
            newProjectal.transform.position = shootingPosition.transform.position;
            newProjectal.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;

            timeSinceLastShoot = 0;
        }
        
        timeSinceLastShoot += Time.deltaTime;
    }
}
