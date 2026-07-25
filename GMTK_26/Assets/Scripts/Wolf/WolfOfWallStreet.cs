using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfOfWallStreet : MonoBehaviour
{

    private WolfMovement wolfMovement;
    [SerializeField] Transform boxCastSize;
    [SerializeField] Transform laser;
    [SerializeField] float laserCooldownTime;


    private bool stopLoopAction;
    private bool loopActionRunning;
    private bool isFireingLaser;
    private bool seePlayer;
    private bool laserOnCooldown;
    private float timeSinceLaser;

    private void Awake()
    {
        wolfMovement = GetComponent<WolfMovement>();
        stopLoopAction = false;
        seePlayer = false;
        laserOnCooldown = false;
        timeSinceLaser = 0f;
        loopActionRunning = false;
    }

    void Start()
    {
        //StartCoroutine(WolfLogic());
        StartCoroutine(WolfPatrol());
    }

    // Update is called once per frame
    void Update()
    {
        seePlayer = LookForPlayer();

        if (seePlayer && !laserOnCooldown && !isFireingLaser)
        {
            stopLoopAction = true;
            isFireingLaser = true;
            StopAllCoroutines();
            loopActionRunning = false;
            StartCoroutine(FireLaserCoroutine());
            laserOnCooldown = true;
            timeSinceLaser = 0f;
        }
        else
        {
            timeSinceLaser += Time.deltaTime;
            if (timeSinceLaser > laserCooldownTime)
            {
                laserOnCooldown = false;
            }
            if (!loopActionRunning && !isFireingLaser)
            {
                StartCoroutine(WolfPatrol());
            }
        }
        //Debug.Log(seePlayer);
    }

    bool LookForPlayer()
    {
        // check line of sight
        float distance = 50f;
        Vector2 origin;
        for(int r = 0; r < boxCastSize.lossyScale.y; r++)
        {
            origin = new Vector2(transform.position.x, boxCastSize.position.y - boxCastSize.lossyScale.y / 2 + r);        
        
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, transform.right, distance);
        
            Debug.DrawRay(origin, transform.right * distance, Color.green);            

            //Debug.Log(hits.Length);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if (!hit.collider.isTrigger && hit.transform.tag == "Player")
                {
                    
                    Debug.DrawRay(origin, transform.right * hit.distance, Color.red);
                    return true;
                }
            }
        }
        return false;
    }
    IEnumerator FireLaserCoroutine()
    {
        isFireingLaser = true;
        wolfMovement._movementCommand = Vector2.zero;
        wolfMovement._jumpCancle = false;
        wolfMovement._jumpStart = false;

        //start laser animation
        yield return new WaitForSeconds(2);

        //fire laser shot
        laser.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        laser.gameObject.SetActive(false);
        //stop laser animation
        yield return new WaitForSeconds(2);

        isFireingLaser = false;
    }
    IEnumerator WolfLogic()
    {
        stopLoopAction = false;
        loopActionRunning = true;
        while (!stopLoopAction)
        {
            wolfMovement._movementCommand = Vector2.right;
            yield return new WaitForSeconds(0.5f);
            wolfMovement._jumpCancle = false;
            wolfMovement._jumpStart = true;
            yield return new WaitForSeconds(0.1f);
            wolfMovement._jumpCancle= true;
            wolfMovement._jumpStart = false;
            yield return new WaitForSeconds(0.5f);
            wolfMovement._movementCommand = Vector2.zero;
            yield return new WaitForSeconds(1);
            wolfMovement._movementCommand = Vector2.left;
            yield return new WaitForSeconds(0.5f);
            wolfMovement._jumpCancle = false;
            wolfMovement._jumpStart = true;
            yield return new WaitForSeconds(0.1f);
            wolfMovement._jumpCancle = true;
            wolfMovement._jumpStart = false;
            yield return new WaitForSeconds(0.5f);
            wolfMovement._movementCommand = Vector2.zero;
            yield return new WaitForSeconds(1);
        }
        loopActionRunning = false;
    }

    IEnumerator WolfPatrol()
    {
        stopLoopAction = false;
        loopActionRunning = true;
        while (!stopLoopAction)
        {
            wolfMovement._movementCommand = Vector2.right;
            yield return new WaitForSeconds(3f);
            wolfMovement._movementCommand = Vector2.zero;
            yield return new WaitForSeconds(1);
            wolfMovement._movementCommand = Vector2.left;
            yield return new WaitForSeconds(3f);
            wolfMovement._movementCommand = Vector2.zero;
            yield return new WaitForSeconds(1);
        }
        loopActionRunning = false;
    }



}
