using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingForbidden : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    private Collider2D overHeadTrigger;

    private int numOfColidersOverhead;

    void Awake()
    {
        overHeadTrigger = GetComponent<BoxCollider2D>();
        numOfColidersOverhead = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (! collision.isTrigger)
        {
            numOfColidersOverhead += 1;
            playerMovement._canStandUp = false;
            //Debug.Log(numOfColidersOverhead);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.isTrigger)
        {
            numOfColidersOverhead -= 1;
            if (numOfColidersOverhead == 0)
            {
                playerMovement._canStandUp = true;
            }
            //Debug.Log(numOfColidersOverhead);
        }
    }

}
