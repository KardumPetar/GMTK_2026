using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfOfWallStreet : MonoBehaviour
{

    private WolfMovement wolfMovement;
    [SerializeField] Transform boxCastSize;
    [SerializeField] Transform laser;
    [SerializeField] Transform AOE;
    private Animator animator;
    [SerializeField] float laserCooldownTime;

    [SerializeField] float jumpAtackCooldownTime;
    [SerializeField] float jumpAtackTargetDistance;
    [SerializeField] float jumpAtackJumpTime;

    private bool stopLoopAction;
    private bool loopActionRunning;
    private bool isAtacking;
    private bool seePlayer;
    private float distanceToPlayer;
    private Vector2 directionToPlayer;
    private float playerY;

    private bool laserOnCooldown;
    private float timeSinceLaser;

    private bool jumpAtackOnCooldown = false;
    private float timeSinceJumpAtack = 0f;

    private void Awake()
    {
        wolfMovement = GetComponent<WolfMovement>();
        stopLoopAction = false;
        seePlayer = false;
        laserOnCooldown = true;
        timeSinceLaser = 0f;
        loopActionRunning = true;
        animator = GetComponentInChildren<Animator>();
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

        if (seePlayer && !isAtacking )
        {
            if (distanceToPlayer > jumpAtackTargetDistance && !jumpAtackOnCooldown)
            {
                StartJumpAtack();
            }      
            else if (!laserOnCooldown)
            {
                FireLaser();
            }

        }

        if (!isAtacking)
        {
            timeSinceLaser += Time.deltaTime;
            if (timeSinceLaser > laserCooldownTime)
            {
                laserOnCooldown = false;
            }
            timeSinceJumpAtack += Time.deltaTime;
            if (timeSinceJumpAtack > jumpAtackCooldownTime)
            {
                jumpAtackOnCooldown = false;
            }

            if (!loopActionRunning && !isAtacking)
            {
                StartCoroutine(WolfPatrol());
            }
        }


        //Debug.Log(seePlayer);
    }
    void FireLaser()
    {
        stopLoopAction = true;
        isAtacking = true;
        StopAllCoroutines();
        loopActionRunning = false;
        laserOnCooldown = true;
        timeSinceLaser = 0f;
        StartCoroutine(FireLaserCoroutine());
    }

    void StartJumpAtack()
    {
        stopLoopAction = true;
        isAtacking = true;
        StopAllCoroutines();
        loopActionRunning = false;
        jumpAtackOnCooldown = true;
        timeSinceJumpAtack = 0f;
        StartCoroutine(JumpAtack());

    }

    bool LookForPlayer()
    {
        // check line of sight
        float distance = 50f;
        Vector2 origin;
        for (int r = 0; r < boxCastSize.lossyScale.y; r++)
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
                    distanceToPlayer = hit.distance;
                    directionToPlayer = transform.right;
                    playerY = origin.y;
                    Debug.DrawRay(origin, transform.right * hit.distance, Color.red);
                    return true;
                }
            }
        }
        return false;
    }
    IEnumerator FireLaserCoroutine()
    {
        isAtacking = true;
        wolfMovement._movementCommand = Vector2.zero;
        wolfMovement._jumpCancle = false;
        wolfMovement._jumpStart = false;

        animator.SetTrigger("shoot");
        //start laser animation
        yield return new WaitForSeconds(1.2f);

        //fire laser shot
        animator.SetTrigger("laserStart");
        laser.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        laser.gameObject.SetActive(false);
        animator.SetTrigger("laserEnd");
        //stop laser animation
        yield return new WaitForSeconds(0.15f);

        isAtacking = false;
    }

    IEnumerator JumpAtack()
    {
        isAtacking = true;
        wolfMovement._movementCommand = Vector2.zero;
        wolfMovement._jumpCancle = false;
        wolfMovement._jumpStart = false;

        yield return new WaitForSeconds(0.5f);
        
        while (distanceToPlayer > jumpAtackTargetDistance && seePlayer)
        {
            wolfMovement._movementCommand = directionToPlayer;
            wolfMovement._runCommand = true;
            yield return 0;
        }
        
        //start jump
        wolfMovement._jumpCancle = false;
        wolfMovement._jumpStart = true;
        yield return new WaitForSeconds(jumpAtackJumpTime);
        wolfMovement._jumpCancle = true;
        wolfMovement._jumpStart = false;

        yield return new WaitForSeconds(0.2f);
        // stop
        wolfMovement._movementCommand = Vector2.zero;
        wolfMovement._runCommand = false;

        //do AOE damage
        AOE.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        AOE.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);
        isAtacking = false;
    }


    IEnumerator WolfPatrol()
    {
        stopLoopAction = false;
        loopActionRunning = true;
        while (!stopLoopAction)
        {
            wolfMovement._movementCommand = Vector2.right;
            yield return new WaitForSeconds(8f);
            wolfMovement._movementCommand = Vector2.zero;
            yield return new WaitForSeconds(2);
            wolfMovement._movementCommand = Vector2.left;
            yield return new WaitForSeconds(8f);
            wolfMovement._movementCommand = Vector2.zero;
            yield return new WaitForSeconds(2);
        }
        loopActionRunning = false;
    }



}
