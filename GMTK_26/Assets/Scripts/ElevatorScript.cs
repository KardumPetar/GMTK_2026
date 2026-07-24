using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    [SerializeField] float[] floorPositions;
    [SerializeField] float _maxSpeed;
    [SerializeField] float acceleration;

    float _currentVelocity;
    float _currentPositon;
    float _targetPosition;

    private Rigidbody2D _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _targetPosition = floorPositions[0];
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MoveToFloor(int floorNumber)
    {
        _targetPosition = floorPositions[floorNumber];


        _currentVelocity = _currentVelocity + acceleration * Time.fixedDeltaTime;

        
        _currentPositon = Mathf.Lerp(_rb.position.y, _targetPosition, _currentVelocity * Time.fixedDeltaTime);

        _rb.MovePosition(new Vector2(0, _currentPositon));
        
    }


}
