using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{

    [SerializeField] float triggerStayTime;

    float stayTime;

    public bool activated;

    void Awake()
    {
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (stayTime >= triggerStayTime)
        {
            activated = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.layer == 7)
        {
            stayTime = 0;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            stayTime += Time.deltaTime;
        }
    }


}
