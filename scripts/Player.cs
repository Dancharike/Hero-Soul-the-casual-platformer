using Godot;
using System;

public partial class Player : CharacterBody2D
{
    private const float Speed = 130.0f;
    private const float JumpVelocity = -300.0f;
    private const float CoyoteTime = 0.2f; 
    private const float DashDistance = 500.0f; 
    private const float DashDuration = 0.1f; //значение, что будет обнулять _dashTime
    private const float DashCooldown = 2f; //задержка между рывками

    private AnimatedSprite2D AnimatedSprite;
    private float _coyoteTimeCounter = 0.0f; 
    private int _jumpCount = 0; 
    private bool _isDashing = false; 
    private float _dashTime = 0.0f; 
    private float _dashCooldownTimer = 0.0f; //таймер, что отслеживает время после рывка

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Hidden;
        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        _dashCooldownTimer -= (float)delta;

        if (!IsOnFloor())
        {
            _coyoteTimeCounter -= (float)delta;
        }
        else
        {
            _coyoteTimeCounter = CoyoteTime; 
            _jumpCount = 0; 
        }

        if (Input.IsActionJustPressed("jump"))
        {
            if (IsOnFloor() || _coyoteTimeCounter > 0) 
            {
                velocity.Y = JumpVelocity;
                _jumpCount++; 
                
                if (_jumpCount == 1)
                {
                    _coyoteTimeCounter = 0; 
                }
            }
            else if (_jumpCount == 1) 
            {
                velocity.Y = JumpVelocity;
                _jumpCount++; 
            }
        }

        Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down").Normalized();
        
        if (Input.IsActionJustPressed("dash") && !_isDashing && _dashCooldownTimer <= 0 && direction != Vector2.Zero)
        {
            _isDashing = true; 
            _dashTime = 0.0f; 
            _dashCooldownTimer = DashCooldown; //сброс таймера
        }

        if (_isDashing)
        {
            Vector2 dashDirection = direction != Vector2.Zero ? direction : GlobalTransform.X.Normalized(); //если нет ввода использует направление взгляда
            velocity = dashDirection * DashDistance; 

            _dashTime += (float)delta;
            if (_dashTime >= DashDuration)
            {
                _isDashing = false; 
            }
        }
        else
        {
            velocity += GetGravity() * (float)delta;

            if (direction.X > 0)
            {
                AnimatedSprite.FlipH = false;
            }
            else if (direction.X < 0)
            {
                AnimatedSprite.FlipH = true;
            }

            if (IsOnFloor())
            {
                if (direction.X == 0)
                {
                    AnimatedSprite.Play("idle");
                }
                else
                {
                    AnimatedSprite.Play("run");
                }
            }
            else
            {
                AnimatedSprite.Play("jump");
            }

            if (direction != Vector2.Zero)
            {
                velocity.X = direction.X * Speed;
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            }
        }

        Velocity = velocity; 
        MoveAndSlide(); 
    }
}
