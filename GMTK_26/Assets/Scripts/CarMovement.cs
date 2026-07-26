using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float unSpawnX;
    [SerializeField] float spawnX;
    private  Rigidbody2D _rb;
    [SerializeField] GameObject colidersGO;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        _rb.velocity = speed * Vector2.left;
    }
    private void Update()
    {
        if (_rb.position.x < unSpawnX)
        {
            _rb.position = new Vector3(spawnX, _rb.position.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            colidersGO.SetActive(false);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            colidersGO.SetActive(true);
        }
    }

}
