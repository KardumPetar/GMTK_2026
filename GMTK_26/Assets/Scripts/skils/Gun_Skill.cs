using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun_Skill : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    private Vector2 direction;
    [SerializeField] private float speed;
    [SerializeField] private float fireRate;

    private void OnMouseDown() {
        
        direction = Input.mousePosition - transform.position;
        GameObject newProjectal  = Instantiate(projectile, transform);
        newProjectal.GetComponent<Rigidbody2D>().velocity = direction*speed;
    }
}
