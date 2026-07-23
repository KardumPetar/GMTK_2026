using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float FollowSpeedX = 2f;
    public float FollowSpeedY = 2f;
    public float yOffset = 1f;
    public Transform target;



    private void OnGUI()
    {
        Vector3 newPosX = new Vector3(target.position.x, transform.position.y, -10f);
        if (Mathf.Abs(newPosX.x  - transform.position.x) > 1)
        {
            transform.position = Vector3.Lerp(transform.position, newPosX, FollowSpeedX * Time.deltaTime);
        }        
        Vector3 newPosY = new Vector3(transform.position.x, target.position.y + yOffset, -10f);
        if (Mathf.Abs(newPosY.y - transform.position.y) > 2f)
        {
            transform.position = Vector3.Lerp(transform.position, newPosY, FollowSpeedY * Time.deltaTime);
        }
    }
}
