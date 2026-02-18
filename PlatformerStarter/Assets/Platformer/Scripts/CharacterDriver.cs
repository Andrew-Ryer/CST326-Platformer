using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterDriver : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float groundAcceleration = 15f;
    
    public float apexHeight = 4.5f;
    public float apexTime = 0.5f;
    
    Vector2 _velocity;
    CharacterController _controller;
    Quaternion facingRight;
    Quaternion facingLeft;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        facingRight = Quaternion.identity;
        facingLeft = Quaternion.Euler(0f, 180f, 0f);
        
        _controller = GetComponent<CharacterController>();
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

        if (_controller.isGrounded)
        {
            if (direction != 0)
            {
                if (Mathf.Sign(direction) != Mathf.Sign(_velocity.x)) 
                { 
                    _velocity.x = 0f; 
                }
                _velocity.x += direction * groundAcceleration * Time.deltaTime;
                _velocity.x = Mathf.Clamp(_velocity.x, -walkSpeed, walkSpeed);

                transform.rotation = (direction > 0f) ? facingRight : facingLeft;
            }
            else 
            { 
                // This kinda feels like ice
                // _velocity.x = 1f - Time.deltaTime * 2f;
                _velocity.x = Mathf.MoveTowards(_velocity.x, 0f, groundAcceleration * Time.deltaTime);
            }

            if (jumpPressedThisFrame)
            {
                _velocity.y = 2f * apexHeight / apexTime;
            }
        }
        else
        {
            if (!jumpHeld)
            {
                gravityModifer = 2f;
            }
        }

        float gravity = 2f * apexHeight / (apexTime * apexTime);
        _velocity.y -= gravity * gravityModifer * Time.deltaTime;
        
        float deltaY = _velocity.y * Time.deltaTime;
        float deltaX = _velocity.x * Time.deltaTime;
        //float deltaX = direction * walkSpeed * Time.deltaTime;
        
        Vector3 deltaPosition = new (deltaX, deltaY, 0f);
        CollisionFlags collisions = _controller.Move(deltaPosition);
        
        if ((collisions & CollisionFlags.CollidedAbove) != 0)
        {
            _velocity.y = 0f;
        }
        
        if ((collisions & CollisionFlags.CollidedSides) != 0)
        {
            _velocity.x = 0f;
        }
        
        //Debug.Log($"Grounded: {_controller.isGrounded}");
    }
}
