using Godot;
using System;

public partial class Obstacle : Area2D
{
    public ObstacleSpawner spawnerParent;
    public Vector2 Direction = Vector2.Left;
    public float Speed = 50f;
    public float lifetime = 0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // Update lifetime
        lifetime += (float)delta;

        // Check if lifetime exceeded and recycle
        if (spawnerParent != null && lifetime >= spawnerParent.ObstaclesMaxLifetime)
        {
            spawnerParent.RecycleObstacle(this);
            return;
        }

        // Move the obstacle indefinitely
        if (Direction != Vector2.Zero)
            Position += Direction.Normalized() * Speed * (float)delta;
    }

    // Reset obstacle properties for reuse
    public void Reset()
    {
        spawnerParent = null;
        Direction = Vector2.Left;
        Speed = 50f;
        lifetime = 0f;
        Position = Vector2.Zero;
    }
}