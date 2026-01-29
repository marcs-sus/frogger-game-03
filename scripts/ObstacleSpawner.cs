using Godot;
using System;

public partial class ObstacleSpawner : Marker2D
{
	[Export] public ObstaclePool ObstaclePool;
	[Export] public byte ObstacleLength = 1;
	[Export] public Vector2 SpawnDirection = Vector2.Left;
	[Export] public float SpawnInterval = 2.0f;
	[Export] public float ObstaclesMaxLifetime = 15.0f;

	private float spawnTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawnTimer = SpawnInterval;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Update spawn timer
		spawnTimer -= (float)delta;

		// Check if it's time to spawn a new obstacle
		if (spawnTimer <= 0)
		{
			SpawnObstacle();
			spawnTimer = SpawnInterval;
		}
	}

	// Instantiate a new obstacle with the assigned scene
	public void SpawnObstacle()
	{
		if (ObstaclePool == null || ObstaclePool.pool.Count <= 0)
		{
			GD.PrintErr("Obstacle pool is not assigned or empty.");
			return;
		}

		Obstacle obstacle = ObstaclePool.GetObstacle();
		if (obstacle == null)
		{
			GD.PrintErr("Failed to instantiate obstacle.");
			return;
		}

		// Set the obstacle's direction and ownership
		obstacle.Direction = SpawnDirection.Normalized();
		obstacle.spawnerParent = this;

		AddChild(obstacle);
	}

	// Recycle an obstacle back to the pool
	public void RecycleObstacle(Obstacle obstacle)
	{
		if (ObstaclePool != null)
		{
			RemoveChild(obstacle);
			ObstaclePool.ReturnObstacle(obstacle);
		}
	}
}
