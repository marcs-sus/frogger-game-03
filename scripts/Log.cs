using Godot;
using System;
using static Globals;

public partial class Log : Obstacle
{
    public Sprite2D sprite => GetNode<Sprite2D>("Sprite2D");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    // Initialize the log with its length and spawn order
    public void Initialize(int logLength, int lengthIndex)
    {
        // Change log sprite region based on the spawn order
        if (lengthIndex == 0) // First log
            if (Direction == Vector2.Left)
                sprite.RegionRect = FIRST_LOG_REGION;
            else
                sprite.RegionRect = LAST_LOG_REGION;
        else if (lengthIndex == logLength - 1) // Last log
            if (Direction == Vector2.Left)
                sprite.RegionRect = LAST_LOG_REGION;
            else
                sprite.RegionRect = FIRST_LOG_REGION;
    }

    // Check if player is on log
    private void _OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            player.platformCount++;
            player.currentPlatform = this;
            player.Velocity = Direction.Normalized() * Speed;
            GD.Print("Player moved onto log");
        }
    }

    // Check if player left log
    private void _OnBodyExited(Node2D body)
    {
        if (body is Player player)
        {
            player.platformCount--;
            if (player.currentPlatform == this)
            {
                player.currentPlatform = null;
            }
            GD.Print("Player left log");
        }
    }
}
