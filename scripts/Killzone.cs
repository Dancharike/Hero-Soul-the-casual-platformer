using Godot;
using System;

public partial class Killzone : Area2D
{
    private Timer _timer;
    private bool _playerIsDead = false;
    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        _timer.Timeout += OnTimerTimeout;
    }

    private void OnBodyEntered(Node body)
    {
        GD.Print("Player died!");
        Engine.TimeScale = 0.5;
        _playerIsDead = true;
        GD.Print("Player is dead: " + _playerIsDead);
        //body.GetNode<CollisionShape2D>("CollisionShape2D").QueueFree();
        _timer.Start();
    }

    private void OnTimerTimeout()
    {
        Engine.TimeScale = 1;
        GetTree().ReloadCurrentScene();
    }

    public bool IsPlayerDead()
    {
        return _playerIsDead;
    }
}
