using Godot;
using System;

public partial class Player : CharacterBody2D
{
    private const float Speed = 130.0f;
    private const float JumpVelocity = -300.0f;
    private const float DashDistance = 500.0f;
    private const float DashDuration = 0.1f; //значение, что будет обнулять _dashTime
    private const float DashCooldown = 2f; //задержка между рывками
    private const float Gravity = 980f;

    private AnimatedSprite2D AnimatedSprite;
    private Movement _movement;
    
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Hidden;
        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _movement = new Movement(Speed, JumpVelocity, DashDistance, DashDuration, DashCooldown, Gravity);
    }
    
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down").Normalized();
        if (Input.IsActionJustPressed("jump"))
        {
            velocity.Y = JumpVelocity;
        }

        if (Input.IsActionJustPressed("dash") && _movement.CanDash(direction))
        {
            _movement.StartDash();
        }
        
        velocity = _movement.HandleMovement(velocity, direction, IsOnFloor(), delta );
        UpdateAnimation(direction);
        Velocity = velocity;
        MoveAndSlide();
    }

    private void UpdateAnimation(Vector2 direction)
    {
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
            if (direction == Vector2.Zero)
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
    }
}
