using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    [SerializeField] PlayerMovement playerScript;


    public Transform otherPortal;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7 && !collision.isTrigger)
        {
            if (!playerScript._isTeleporting)
            {
                playerScript._isTeleporting = true;
                playerScript._rb.velocity = Vector2.zero;
                playerScript._initiateExternalTeleport = true;
                playerScript._externalTeleportTarget = otherPortal.position;
            }
            
        }
    }



}
