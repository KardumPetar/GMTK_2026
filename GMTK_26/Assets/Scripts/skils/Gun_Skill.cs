using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun_Skill : Skill
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject shootingPosition;
    private Vector2 direction;
    [SerializeField] private float speed;
    
    [SerializeField] private float cooldown_time;
    private float timeSinceLastShoot;
    public static PlayerInput PlayerInput;
    private InputAction _shoot;
    private GameObject projectalsGO;

    private bool fixed_allowed = false;
    private bool mouse_allowed = false;

    public override void Allow(string name) {
        switch (name) {
            case "fixed":
                fixed_allowed = true; break;
            case "mouse":
                fixed_allowed = false;
                mouse_allowed = true; break;

        }
    }
    private void Awake() {
        PlayerInput = FindAnyObjectByType<PlayerInput>(FindObjectsInactive.Include);
        _shoot = PlayerInput.actions["Shoot"];
        timeSinceLastShoot = 0;
        projectalsGO = GameObject.Find("Projectals");
    }

    
    void Update() {
        
        if (_shoot.IsPressed() && timeSinceLastShoot>cooldown_time) {
            if (fixed_allowed) {
                direction = transform.TransformVector(Vector2.right);
            }
            else if (mouse_allowed) {
                direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition)-shootingPosition.transform.position);
                direction  = direction.normalized;
            }
            if (fixed_allowed || mouse_allowed) {
                GameObject newProjectal = Instantiate(projectile, projectalsGO.transform);
                newProjectal.transform.position = shootingPosition.transform.position;
                newProjectal.GetComponent<Rigidbody2D>().velocity = direction * speed;

                timeSinceLastShoot = 0;
            }
        }
        timeSinceLastShoot += Time.deltaTime;
    }
}
