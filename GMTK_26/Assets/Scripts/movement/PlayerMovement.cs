using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : Skill
{
    [Header("References")]
    public PlayerMovementStats MoveStats;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private GameObject _standing_body;
    [SerializeField] private GameObject _crouched_body;
    [SerializeField] private Animator animator;
    [SerializeField] Transform teleport_start_position;
    [SerializeField] private GameObject _teleportPrewiev;

    [SerializeField] Color _teleportValidColor;
    [SerializeField] Color _teleportInvalidColor;

    private Collider2D _standing_bodyColl;
    private Collider2D _crouched_bodyColl;
    private Collider2D _bodyColl;


    [SerializeField] bool right_move_allowed = false;
    [SerializeField] bool left_move_allowed = false;
    [SerializeField] bool jump_allowed = false;
    [SerializeField] bool run_allowed = false;
    [SerializeField] bool crouch_allowed = false;
    [SerializeField] bool teleport_allowed = false;

    private Rigidbody2D _rb;

    //movement vars
    private Vector2 _moveVelocity;
    private bool _isFacingRight;

    //collision check vars
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool _bumpedHead;

    //jump vars
    public float VerticalVelocity { get; private set; }
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _numberOfJumpsUsed;

    //multi jump
    public int _numOfJumps  = 1;

    //apex vars
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    //jump buffer vars
    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;

    //coyote time vars
    private float _coyoteTimer;

    //crouching
    private bool _isCrouching;

    public bool _canStandUp;

    //teleporting
    private Vector2 _teleportTarget;
    private bool _initiateTeleport;
    private bool _isTeleporting;
    [SerializeField] private float _teleportationTime = 0.5f;

    public override void Allow(string name) {
        switch (name) {
            case "right_move":
                right_move_allowed = true;break;
            case "left_move":
                left_move_allowed = true; break;
            case "jump":
                jump_allowed = true; break;
            case "run":
                run_allowed = true; break;
            case "double_jump":
                _numOfJumps = 2; break;
            case "crouch":
                crouch_allowed = true; break;
            case "teleport":
                teleport_allowed = true; break;
        }
    }
    private void Awake()
    {
        _isFacingRight = true;
        _isCrouching = false;
        _canStandUp = true;
        _initiateTeleport = false;
        _teleportTarget = Vector2.negativeInfinity;
        _rb = GetComponent<Rigidbody2D>();
        _standing_bodyColl = _standing_body.GetComponent<CapsuleCollider2D>();
        _crouched_bodyColl = _crouched_body.GetComponent<CapsuleCollider2D>();
        _bodyColl = _standing_bodyColl;
    }

    private void Update()
    {
        if (!_isTeleporting)
        {
            CountTimers();
            JumpChecks();
            if (crouch_allowed)
            {
                CrouchCheck(InputManager.CrouchIsHeld);
            }
            if (teleport_allowed && Input.GetMouseButton(1))
            {
                GetTeleportTarget();
            }
            else
            {
                CancleTeleport();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_isTeleporting)
        {
            CollisionChecks();
            Jump();
                            
            if (_isGrounded)
            {
                Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, InputManager.Movement);
            }
            else
            {
                Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Movement);
            }

            if (teleport_allowed && _initiateTeleport)
            {
                //TeleportTo(_teleportTarget);
                StartCoroutine(TeleportToIEnum(_teleportTarget));
            }
        }
        

    }

    #region Teleport
    void GetTeleportTarget()
    {

        // find teleport Target
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = target - new Vector2(teleport_start_position.position.x, teleport_start_position.position.y);
        
        _teleportPrewiev.SetActive(true);
        _teleportPrewiev.transform.position = target;

        // check line of sight
        float distance = direction.magnitude;
        direction = direction.normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(teleport_start_position.position, direction, distance);
        Debug.DrawRay(teleport_start_position.position, direction * distance, Color.green);
        bool haveLineOfSight = true;

        //Debug.Log(hits.Length);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];
            if (!hit.collider.isTrigger && hit.transform.tag != "Player")
            {
                haveLineOfSight = false;
                Debug.DrawRay(teleport_start_position.position, direction  * hit.distance, Color.red);
            }
        }
        Debug.Log(haveLineOfSight);
        if (haveLineOfSight)
        {
            _teleportPrewiev.GetComponent<SpriteRenderer>().color = _teleportValidColor;
            _teleportTarget = target;
            if (Input.GetMouseButtonDown(0))
            {
                _initiateTeleport = true;
            }
        }
        else
        {
            _teleportPrewiev.GetComponent<SpriteRenderer>().color = _teleportInvalidColor;
            _initiateTeleport = false;
            _teleportTarget = Vector2.negativeInfinity;
        }        

    }

    void CancleTeleport()
    {
        _teleportPrewiev.SetActive(false);
        _initiateTeleport = false;
        _teleportTarget = Vector2.negativeInfinity;
    }

    void TeleportTo(Vector3 targetPosition)
    {
        _isTeleporting = true;
        animator.SetTrigger("batIn");
        _teleportPrewiev.SetActive(false);
        transform.position = targetPosition;
        _initiateTeleport = false;

    }
    IEnumerator TeleportToIEnum(Vector3 targetPosition) {
        _isTeleporting = true;
        _initiateTeleport = false;
        _teleportPrewiev.SetActive(false);

        animator.SetTrigger("batIn");      
        
        yield return new WaitForSeconds(0.5f);

        if (_isCrouching)
        {
            _crouched_body.SetActive(false);
        }
        else
        {
            _standing_body.SetActive(false);
        }
        _feetColl.gameObject.SetActive(false); 

        linearTeleport(transform.position, targetPosition, _teleportationTime);
        yield return new WaitForSeconds(_teleportationTime);
        _rb.velocity = Vector3.zero;

        if (_isCrouching)
        {
            _crouched_body.SetActive(true);
        }
        else
        {
            _standing_body.SetActive(true);
        }
        _feetColl.gameObject.SetActive(true);

        animator.SetTrigger("batOut");

        yield return new WaitForSeconds(0.5f);
        

        _isTeleporting = false;
    }

    void linearTeleport(Vector3 startPosition, Vector3 targetPosition, float teleportationTime)
    {
        _rb.velocity = (targetPosition - startPosition) / teleportationTime;
    }


    #endregion


    #region Movement

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (!right_move_allowed)
        { moveInput.x = Mathf.Min(moveInput.x, 0f); }
        if (!left_move_allowed)
        { moveInput.x = Mathf.Max(moveInput.x, 0f); }

        if (moveInput != Vector2.zero)
        {
            if (moveInput.x  != 0)
            {
                animator.SetBool("isRunning", true);
            }
            TurnCheck(moveInput);
                        

            Vector2 targetVelocity = Vector2.zero;
            if (InputManager.RunIsHeld && run_allowed)
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxRunSpeed;
            }
            else { targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed; }

            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
        }
        else if (moveInput == Vector2.zero)
        {
            Turn();
            animator.SetBool("isRunning", false);
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (_isFacingRight && moveInput.x < 0) {
            Turn(false);
        }
        else if (!_isFacingRight && moveInput.x > 0) {
            Turn(true);
        }

    }

    private void Turn(bool turnRight)
    {
        if (turnRight && !_isFacingRight)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (!turnRight && _isFacingRight) {
        
            _isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }
    private void Turn() {
        Vector2  direction = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position));
        
        if(direction.x < 0) {
            
            Turn(false);
        }
        else if (direction.x > 0) {
            Turn(true);
        }
    }
    private void CrouchCheck(bool crouching_input)
    {
        if (!_isCrouching && crouching_input)
        {            
            _isCrouching = true;
            _standing_body.SetActive(false);
            _crouched_body.SetActive(true);
            _bodyColl = _crouched_bodyColl;

            animator.SetTrigger("crouchIn");
        }
        else if (_isCrouching && !crouching_input && _canStandUp)
        {
            _isCrouching = false;
            _standing_body.SetActive(true);
            _crouched_body.SetActive(false);
            _bodyColl = _standing_bodyColl;
            animator.SetTrigger("crounchOut");
        }

    }

    #endregion
    
    #region Jump

    private void JumpChecks()
    {
        //press
        if (InputManager.JumpWasPressed && jump_allowed)
        {
            _jumpBufferTimer = MoveStats.JumpBufferTime;
            _jumpReleasedDuringBuffer = false;
        }
        //release
        if (InputManager.JumpWasReleased && jump_allowed)
        {
            if (_jumpBufferTimer > 0f)
            {
                _jumpReleasedDuringBuffer = true;
            }
            if (_isJumping  && VerticalVelocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = MoveStats.TimeForUpwardsCancel;
                    VerticalVelocity = 0;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = VerticalVelocity;
                }
            }
        }
        //buffering and coyote
        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
        {
            InitiateJump(1);

            if (_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }
        //double jump
        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < _numOfJumps)
        {
            _isFastFalling = false;
            InitiateJump(1);
        }
        //air jump after coyote
        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < _numOfJumps - 1)
        {
            InitiateJump(2);
            _isFastFalling = false;
        }
        //landed
        if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 00f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            _isPastApexThreshold = false;
            _numberOfJumpsUsed = 0;

            VerticalVelocity = Physics2D.gravity.y;

        }
    }

    private void InitiateJump(int numberOfJumpsUsed)
    {
        if (!_isJumping)
        {
            _isJumping = true;
        }

        _jumpBufferTimer = 0;
        _numberOfJumpsUsed += numberOfJumpsUsed;
        VerticalVelocity = MoveStats.InitialJumpVelocity;
    }


    private void Jump()
    {
        //apply gravity while jumping
        if (_isJumping)
        {
            // head bump
            if (_bumpedHead)
            {
                _isFastFalling = true;
            }
            // gravity ascending
            if (VerticalVelocity >= 0f)
            {
                animator.SetBool("isJumpingUp", true);
                animator.SetBool("isJumpingDown", false);
                // apex
                _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

                if (_apexPoint > MoveStats.ApexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }

                    if (_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        if (_timePastApexThreshold < MoveStats.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else
                        {
                            VerticalVelocity = -0.01f;
                        }
                    }
                }
                //gravity ascending but not past apex
                else
                {
                    VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }

            }
            //gravity descending
            else if (!_isFastFalling)
            {
                animator.SetBool("isJumpingUp", false);
                animator.SetBool("isJumpingDown", true);
                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (VerticalVelocity < 0f)
            {
                animator.SetBool("isJumpingUp", false);
                animator.SetBool("isJumpingDown", true);
                if (!_isFalling)
                {
                    _isFalling = true;
                }
            }
        }
        else
        {
            animator.SetBool("isJumpingUp", false);
            animator.SetBool("isJumpingDown", false);
        }
            //jump cut
        if (_isFastFalling)
        {
            if (_fastFallTime >= MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else  if (_fastFallTime < MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MoveStats.TimeForUpwardsCancel));
            }
            _fastFallTime += Time.fixedDeltaTime;
        }

        //gravity falling
        if (!_isGrounded && !_isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }
            VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
        }
        //clamp fall speed
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 50f);

        _rb.velocity = new Vector2(_rb.velocity.x, VerticalVelocity);


    }

    #endregion

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDetectionRayLength);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);
        if (_headHit.collider != null)
        {
            _bumpedHead = true;
        }
        else { _bumpedHead = false; }

        //visualization
    }

    #region Collision Checks

    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);
        if (_groundHit.collider != null)
        {
            _isGrounded = true;
        }
        else { _isGrounded = false; }

        //debug visualisation
    }

    private void CollisionChecks()
    {
        IsGrounded();
        BumpedHead();
    }
    #endregion

    #region Timers

    private  void CountTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;

        if (!_isGrounded)
        {
            _coyoteTimer -= Time.deltaTime;
        }
        else { _coyoteTimer = MoveStats.JumpCoyoteTime; }
    }

    #endregion
}