using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    [SerializeField] float[] floorPositions;
    [SerializeField] GameObject[] floorDoors;
    [SerializeField] float _maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float accelerationTime;

    [SerializeField] GameObject openDoor;
    [SerializeField] GameObject closedDoor;

    float _currentSpeed;
    float _nextPositon;
    float _targetPosition;
    int nextFloor;
    private bool arrived;
    private bool executeMove;
    bool goingUp;
    [SerializeField] private float stopingDistance;

    private Rigidbody2D _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _targetPosition = floorPositions[0];
        _currentSpeed = 0;
        arrived = true;
        executeMove = false;
    }

    void Start()
    {
        nextFloor = findClosestFloor();
        //Debug.Log(nextFloor);
        goingUp = false;
        stopingDistance = _maxSpeed * accelerationTime + Mathf.Abs(acceleration) * Mathf.Pow(accelerationTime, 2) / 2;
        while (stopingDistance > (floorPositions[1] - floorPositions[0])/2.5f)
        {
            accelerationTime *= 0.9f;
            stopingDistance = _maxSpeed * accelerationTime + Mathf.Abs(acceleration) * Mathf.Pow(accelerationTime, 2) / 2;
        }
        StartCoroutine(ElevatorLoop());
    }
    int findClosestFloor()
    {
        float minDistance = Mathf.Infinity;
        float distance;
        int closestFloor = 0;
        for(int i = 0; i < floorPositions.Length; i++) 
        {
            distance = Mathf.Abs(floorPositions[i] - _rb.position.y);
            
            if (distance < minDistance)
            {
                minDistance = distance;
                closestFloor = i;
            }
        }
        return closestFloor;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (executeMove)
        {
            MoveToFloor(nextFloor);
            executeMove = false;
        }
    }

    private void MoveToFloor(int floorNumber)
    {
        _targetPosition = floorPositions[floorNumber];        
        float remainingDistance = _targetPosition - _rb.position.y;
        //Debug.Log(_targetPosition);

        if (Mathf.Abs(remainingDistance) < _maxSpeed * Time.fixedDeltaTime)
        {
            _rb.MovePosition(new Vector2(_rb.position.x, _targetPosition));
            _currentSpeed = 0;
            arrived = true;
        }
        else  
        {
            //Debug.Log(remainingDistance);

            if (Mathf.Abs(remainingDistance) < stopingDistance)
            {
                acceleration = -Mathf.Sign(remainingDistance) * Mathf.Abs(acceleration);
            }
            else
            {
                acceleration = Mathf.Sign(remainingDistance) * Mathf.Abs(acceleration);
            }
            _currentSpeed = _currentSpeed + acceleration * Time.fixedDeltaTime;
            _currentSpeed = Mathf.Sign(remainingDistance) * Mathf.Abs(_currentSpeed);
            _currentSpeed = Mathf.Clamp(_currentSpeed, -_maxSpeed, _maxSpeed);

            //Debug.Log(_currentSpeed);
            _nextPositon = _rb.position.y + _currentSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(new Vector2(_rb.position.x, _nextPositon));
                        
        }       
        
    }

    private void OpenDoor(int floor)
    {
        openDoor.SetActive(true);
        closedDoor.SetActive(false);
        floorDoors[floor].SetActive(false);
    }
    private void CloseDoor(int floor)
    {        
        closedDoor.SetActive(true);
        openDoor.SetActive(false);
        floorDoors[floor].SetActive(true);
    }
    IEnumerator ElevatorLoop()
    {
        
        

        while (true) {
            //Debug.Log(nextFloor);
            while (!arrived)
            {
                executeMove = true;
                
                yield return 0;
            }
            
            yield return new WaitForSeconds(1f);
            OpenDoor(nextFloor);
            yield return new WaitForSeconds(3f);
            CloseDoor(nextFloor);
            yield return new WaitForSeconds(1f);
            
            if (nextFloor == 0 && !goingUp)
            {
                goingUp = true;
            }
            else if (nextFloor == floorPositions.Length - 1 && goingUp)
            {
                goingUp = false;
            }

            if (goingUp)
            {
                nextFloor += 1;
            }
            else
            {
                nextFloor -= 1;
            }
            arrived = false;
        }

    }




}
