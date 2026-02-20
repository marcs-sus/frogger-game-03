using Godot;
using System;
using System.Collections.Generic;

public partial class ObstaclePool : Node
{
    [Export] public PackedScene ObstacleScene;
    [Export] public int PoolSize = 10;

    public Queue<Obstacle> pool = new Queue<Obstacle>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Pre-instantiate a pool of obstacles
        for (int i = 0; i < PoolSize; i++)
        {
            Obstacle obstacle = ObstacleScene.Instantiate<Obstacle>();
            obstacle.originPool = this;
            DeactivateObstacle(obstacle);
            pool.Enqueue(obstacle);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    // Retrieve an obstacle from the pool
    public Obstacle GetObstacle()
    {
        if (pool.Count > 0)
        {
            Obstacle obstacle = pool.Dequeue();
            ActivateObstacle(obstacle);
            return obstacle;
        }
        else
        {
            GD.PrintErr("No obstacles available in the pool!");
            return null;
        }
    }

    // Return an obstacle back to the pool
    public void ReturnObstacle(Obstacle obstacle)
    {
        if (obstacle != null)
        {
            DeactivateObstacle(obstacle);
            pool.Enqueue(obstacle);
        }
    }

    // Hide and disable an obstacle
    private void DeactivateObstacle(Obstacle obstacle)
    {
        obstacle.Hide();
        obstacle.SetProcessMode(ProcessModeEnum.Disabled);
        obstacle.Reset();
    }

    // Re-activate an obstacle
    private void ActivateObstacle(Obstacle obstacle)
    {
        obstacle.Show();
        obstacle.SetProcessMode(ProcessModeEnum.Inherit);
    }
}