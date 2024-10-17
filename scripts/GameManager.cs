using Godot;
using System;

public partial class GameManager : Node
{
    public int score = 0;

    public void AddPoint()
    {
        score += 1;
        GD.Print($"current score is: {score}");
    }
}
