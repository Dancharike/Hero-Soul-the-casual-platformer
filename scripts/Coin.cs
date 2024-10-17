using Godot;
using System;

public partial class Coin : Area2D
{
    private GameManager _gameManager;
    private AudioStreamPlayer2D _audioPlayer; 

    public override void _Ready()
    {
        _gameManager = GetTree().Root.GetNodeOrNull<GameManager>("Game/GameManager");
        if (_gameManager == null)
        {
            GD.Print("Game Manager not found!");
        }
        else
        {
            Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));   
        }
        _audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        if (_audioPlayer == null)
        {
            GD.Print("AudioStreamPlayer2D not found!");
        }
    }

    private void OnBodyEntered(Node body)
    {
        GD.Print("+1 coin!");
        _gameManager.AddPoint();
        if (_audioPlayer != null)
        {
            _audioPlayer.Play();
        }
        QueueFree();
    }
}