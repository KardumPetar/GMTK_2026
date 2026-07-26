using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfMovement : MonoBehaviour
{
    [Header("References")]
    public WolfMovementStats MoveStats;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private GameObject _standing_body;
    [SerializeField] private Animator animator;


    private Collider2D _bodyColl;


    public Rigidbody2D _rb;

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

    //apex vars
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    //jump buffer vars
    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;

    //coyote time vars
    private float _coyoteTimer;

    //movement commands
    public bool _jumpStart;
    public bool _jumpCancle;

    public Vector2 _movementCommand;
    public bool _runCommand;



    private void Awake()
    {
        _isFacingRight = true;
        _rb = GetComponent<Rigidbody2D>();
        _bodyColl = _standing_body.GetComponent<CapsuleCollider2D>();

        _jumpStart = false;
        _jumpCancle = false;
        _runCommand = false;
        _movementCommand = Vector2.zero;
    }

    private void Update()
    {
        CountTimers();
        JumpChecks();        
    }

    private void FixedUpdate()
    {        
        CollisionChecks();
        Jump();

        if (_isGrounded)
        {
            Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, _movementCommand);
        }
        else
        {
            Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, _movementCommand);
        }
    

    }

    
    #region Movement

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {

        if (moveInput != Vector2.zero)
        {
            if (moveInput.x != 0)
            {
                animator.SetBool("isRunning", true);
            }
            TurnCheck(moveInput);


            Vector2 targetVelocity = Vector2.zero;
            if (_runCommand)
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxRunSpeed;
            }
            else { targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed; }

            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
        }
        else if (moveInput == Vector2.zero)
        {
            //Turn();
            animator.SetBool("isRunning", false);
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (_isFacingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if (!_isFacingRight && moveInput.x > 0)
        {
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
        else if (!turnRight && _isFacingRight)
        {

            _isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }
    private void Turn()
    {
        Vector2 direction = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position));

        if (direction.x < 0)
        {

            Turn(false);
        }
        else if (direction.x > 0)
        {
            Turn(true);
        }
    }
    

    #endregion

    #region Jump

    private void JumpChecks()
    {
        //press
        if (_jumpStart)
        {
            _jumpBufferTimer = MoveStats.JumpBufferTime;
            _jumpReleasedDuringBuffer = false;
        }
        //release
        if (_jumpCancle)
        {
            if (_jumpBufferTimer > 0f)
            {
                _jumpReleasedDuringBuffer = true;
            }
            if (_isJumping && VerticalVelocity > 0f)
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
        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed)
        {
            _isFastFalling = false;
            InitiateJump(1);
        }
        //air jump after coyote
        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed - 1)
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
            animator.SetBool("isJumpingUp", false);
            animator.SetBool("isJumpingDown", true);
            if (_fastFallTime >= MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (_fastFallTime < MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MoveStats.TimeForUpwardsCancel));
            }
            _fastFallTime += Time.fixedDeltaTime;
        }

        //gravity falling
        if (!_isGrounded && !_isJumping)
        {
            animator.SetBool("isJumpingUp", false);
            animator.SetBool("isJumpingDown", true);
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

    private void CountTimers()
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
