using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterDriver : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float groundAcceleration = 15f;
    
    public float apexHeight = 4.5f;
    public float apexTime = 0.5f;
    
    // PT2: run / air control / stop tuning
    public float groundDeceleration = 25f;
    public float airAcceleration = 10f;
    public float airMaxSpeed = 8f;
    public float airDeceleration = 2f;

    // PT2: variable jump height (jump cut) + separate X/Y clamp
    [Range(0.1f, 1f)]
    public float jumpCutMultiplier = 0.5f;
    public float maxRiseSpeed = 18f;
    public float maxFallSpeed = 25f;
    
    public ClickRaycast clickRaycast;
    public float headCheckDistance = 0.25f;
    
    Vector2 _velocity;
    CharacterController _controller;
    Animator _animator;
    Quaternion facingRight;
    Quaternion facingLeft;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        facingRight = Quaternion.Euler(0f, 90f, 0f);
        facingLeft = Quaternion.Euler(0f, 270f, 0f);
        
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        
        clickRaycast = FindFirstObjectByType<ClickRaycast>();
    }

    // Update is called once per frame
    void Update()
    {
        float gravityModifer = 1f;
        
        float direction = 0f;
        
        if (Keyboard.current.dKey.isPressed)
        {
            direction += 1f;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            direction -= 1f;
        }
        
        bool jumpPressedThisFrame = Keyboard.current.spaceKey.wasPressedThisFrame;
        bool jumpHeld = Keyboard.current.spaceKey.isPressed;
        
        // PT2: run key and chosen max horizontal speed
        bool runHeld = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
        float maxSpeedX = runHeld ? runSpeed : walkSpeed;

        // Stuff from class
        // basic ground move
        if (_controller.isGrounded)
        {
            if (direction != 0)
            {
                if (Mathf.Sign(direction) != Mathf.Sign(_velocity.x)) 
                { 
                    _velocity.x = 0f; 
                }
                _velocity.x += direction * groundAcceleration * Time.deltaTime;
                //_velocity.x = Mathf.Clamp(_velocity.x, -walkSpeed, walkSpeed);
                _velocity.x = Mathf.Clamp(_velocity.x, -walkSpeed, walkSpeed);

                transform.rotation = (direction > 0f) ? facingRight : facingLeft;
            }
            else 
            { 
                // This kinda feels like ice
                // _velocity.x = 1f - Time.deltaTime * 2f;
                _velocity.x = Mathf.MoveTowards(_velocity.x, 0f, groundAcceleration * Time.deltaTime);
            }

            // jump
            if (jumpPressedThisFrame)
            {
                _velocity.y = 2f * apexHeight / apexTime;
            }
        }
        else
        {
            // long jump
            if (!jumpHeld)
            {
                gravityModifer = 2f;
            }
        }
        
        // PT2: change direction in mid-air
        if (direction != 0f)
        {
            // accelerate horizontally while airborne
            _velocity.x = Mathf.MoveTowards(_velocity.x, direction * airMaxSpeed, airAcceleration * Time.deltaTime);
            transform.rotation = (direction > 0f) ? facingRight : facingLeft;
        }
        else
        {
            // different air move decelerations
            //_velocity.x = Mathf.MoveTowards(_velocity.x, 0f, airDeceleration * Time.deltaTime);
            _velocity.x = Mathf.MoveTowards(_velocity.x, 0f, groundDeceleration * Time.deltaTime);
        }

        float gravity = 2f * apexHeight / (apexTime * apexTime);
        _velocity.y -= gravity * gravityModifer * Time.deltaTime;
        
        // PT2: different variable jump height that I can change while playing
        if (!jumpHeld && _velocity.y > 0f)
        {
            _velocity.y *= jumpCutMultiplier;
        }
        // PT2: separate y clamp
        _velocity.y = Mathf.Clamp(_velocity.y, -maxFallSpeed, maxRiseSpeed);
        
        float deltaY = _velocity.y * Time.deltaTime;
        float deltaX = _velocity.x * Time.deltaTime;
        //float deltaX = direction * walkSpeed * Time.deltaTime;
        
        Vector3 deltaPosition = new (deltaX, deltaY, 0f);
        CollisionFlags collisions = _controller.Move(deltaPosition);
        
        //PT2: Break Bricks
        if ((collisions & CollisionFlags.CollidedAbove) != 0 && _velocity.y > 0f)
        {
            TryBreakBrickAboveHead();
        }
        
        if ((collisions & CollisionFlags.CollidedAbove) != 0)
        {
            _velocity.y = 0f;
        }
        
        if ((collisions & CollisionFlags.CollidedSides) != 0)
        {
            _velocity.x = 0f;
        }
        
        //Debug.Log($"Grounded: {_controller.isGrounded}");
        
        _animator.SetFloat("Speed", Mathf.Abs(_velocity.x));
        _animator.SetBool("Grounded", _controller.isGrounded);
    }
    
    //PT2: break bricks
    void TryBreakBrickAboveHead()
    {
        Vector3 top = transform.position + _controller.center + Vector3.up * (_controller.height * 0.5f - _controller.radius);

        float radius = _controller.radius * 0.9f;

        if (Physics.SphereCast(top, radius, Vector3.up, out RaycastHit hitInfo, headCheckDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            clickRaycast.DestroyBrick(hitInfo.collider.gameObject);
        }
    }
}
