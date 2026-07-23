using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float FollowSpeedX = 2f;
    public float FollowSpeedY = 2f;
    public float yOffset = 1f;
    public Transform target;


    void Update() {
        
        Vector3 newPosX = new Vector3(target.position.x, transform.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPosX, FollowSpeedX * Time.deltaTime);
        Vector3 newPosY = new Vector3(transform.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPosY, FollowSpeedY * Time.deltaTime);
    }

}
