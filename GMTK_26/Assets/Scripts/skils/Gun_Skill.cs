using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun_Skill : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject shootingPosition;
    private Vector2 direction;
    [SerializeField] private float speed;
    
    [SerializeField] private float cooldown_time;
    private float timeSinceLastShoot;
    public static PlayerInput PlayerInput;
    private InputAction _shoot;

    private void Awake() {
        PlayerInput = FindAnyObjectByType<PlayerInput>(FindObjectsInactive.Include);
        _shoot = PlayerInput.actions["Shoot"];
        timeSinceLastShoot = 0;
    }

    
    void Update() {
        if (_shoot.IsPressed() && timeSinceLastShoot>cooldown_time) {
            direction = Vector2.right * speed;
            GameObject newProjectal = Instantiate(projectile, shootingPosition.transform);
            newProjectal.GetComponent<Rigidbody2D>().velocity = direction * speed;

            timeSinceLastShoot = 0;
        }
        timeSinceLastShoot += Time.deltaTime;
    }
}
