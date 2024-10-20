using Godot;
using System;

public partial class Movement : Node
{
    private float Speed { get; set; }
    private float JumpVelocity { get; set; }
    private float DashDistance { get; set; }
    private float DashDuration { get; set; }
    private float DashCooldown { get; set; }
    private float Gravity { get; set; }
    private const float CoyoteTime = 0.2f;

    private float _dashTime = 0.0f;
    private bool _isDashing = false;
    private float _dashCooldownTimer = 0.0f;
    private float _coyoteTimeCounter = 0.0f;
    private int _jumpCount = 0;
    private bool _hasDoubleJumped = false;

    public Movement(float speed, float jumpVelocity, float dashDistance, float dashDuration, float dashCooldown, float gravity)
    {
        Speed = speed;
        JumpVelocity = jumpVelocity;
        DashDistance = dashDistance;
        DashDuration = dashDuration;
        DashCooldown = dashCooldown;
        Gravity = gravity;
    }

    public Vector2 HandleMovement(Vector2 velocity, Vector2 direction, bool isOnFloor, double delta)
    {
        _dashCooldownTimer -= (float)delta;
        if (isOnFloor == true)
        {
            _coyoteTimeCounter = CoyoteTime;  
            //GD.Print("coyote time: " + _coyoteTimeCounter);
            _jumpCount = 0;                   
            _hasDoubleJumped = false;          
        }
        else
        {
            _coyoteTimeCounter -= (float)delta;  
            //GD.Print("coyote time: " + _coyoteTimeCounter);
        }
        
        if (Input.IsActionJustPressed("jump"))
        {
            if (isOnFloor == true || _coyoteTimeCounter > 0)
            {
                GD.Print("coyote time: " + _coyoteTimeCounter);
                velocity.Y = JumpVelocity;
                _jumpCount++;
                _coyoteTimeCounter = 0;  //обнуление койота после прыжка
                GD.Print("coyote time: " + _coyoteTimeCounter);
            }
            else if (_jumpCount == 1 && !_hasDoubleJumped)
            {
                velocity.Y = JumpVelocity;
                _hasDoubleJumped = true;  //ограничение двойного прыжка
            }
        }
        
        if (Input.IsActionJustPressed("dash") && CanDash(direction))
        {
            StartDash();
        }

        if (_isDashing)
        {
            velocity = direction * DashDistance;
            _dashTime += (float)delta;
            if (_dashTime >= DashDuration)
            {
                _isDashing = false;
            }
        }
        else if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = 0;
        }
        
        if (!isOnFloor)
        {
            velocity.Y += Gravity * (float)delta;
        }

        return velocity;
    }

    public bool CanDash(Vector2 direction)
    {
        return !_isDashing && _dashCooldownTimer <= 0 && direction != Vector2.Zero;
    }

    public void StartDash()
    {
        _isDashing = true;
        _dashTime = 0.0f;
        _dashCooldownTimer = DashCooldown;
    }

    public bool IsDashing => _isDashing;
}
