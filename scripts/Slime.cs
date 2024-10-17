using Godot;
using System;

public partial class Slime : Node2D
{
    private const float Speed = 60f;
    private int _direction = 1;

    private RayCast2D _RayCastRight;
    private RayCast2D _RayCastLeft;
    private AnimatedSprite2D AnimatedSprite;

    public override void _Ready()
    {
        _RayCastRight = GetNode<RayCast2D>("RayCast1");
        _RayCastLeft = GetNode<RayCast2D>("RayCast2");
        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
    
    public override void _Process(double delta)
    {
        if (_RayCastRight.IsColliding())
        {
            _direction = -1;
            AnimatedSprite.FlipH = true;
        }

        if (_RayCastLeft.IsColliding())
        {
            _direction = 1;
            AnimatedSprite.FlipH = false;
        }

        Position += new Vector2(_direction * Speed * (float)delta, 0);
    }
}