using Godot;
using System;
using System.Net.Security;

public enum Direction
{
	Right = 1,
	Left = -1,
}

public partial class ObstacleSpawner : Marker2D
{
	[Export] public ObstaclePool ObstaclePool;
	[Export] public byte ObstacleLength = 1;
	[Export] public float ObstacleSpeed = 50f;
	[Export] public Direction SpawnDirection = Direction.Left;
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
			// Spawn a single obstacle
			if (ObstacleLength == 1)
			{
				SpawnObstacle();
				spawnTimer = SpawnInterval;
				return;
			}

			// Spawn multiple obstacles
			for (int i = 0; i < ObstacleLength; i++)
			{
				Obstacle obstacle = SpawnObstacle(Globals.TILE_SIZE * i);

				switch (obstacle)
				{
					case Log log:
						// Change log sprite region based on the spawn order
						if (i == 0) // First log
							log.sprite.RegionRect = new Rect2(0, 0, Globals.TILE_SIZE, Globals.TILE_SIZE);
						else if (i == ObstacleLength - 1) // Last log
							log.sprite.RegionRect = new Rect2(Globals.TILE_SIZE * 2, 0, Globals.TILE_SIZE, Globals.TILE_SIZE);
						break;
				}
			}

			spawnTimer = SpawnInterval * ObstacleLength;
		}
	}

	// Instantiate a new obstacle with the assigned scene and return it
	public Obstacle SpawnObstacle(float offset = 0f)
	{
		if (ObstaclePool == null || ObstaclePool.pool.Count <= 0)
		{
			GD.PrintErr("Obstacle pool is not assigned or empty.");
			return null;
		}

		Obstacle obstacle = ObstaclePool.GetObstacle();
		if (obstacle == null)
		{
			GD.PrintErr("Failed to instantiate obstacle.");
			return null;
		}

		// Set the obstacle's direction and ownership
		obstacle.Position += new Vector2(offset, 0);
		obstacle.Direction = new Vector2((float)SpawnDirection, 0);
		obstacle.Speed = ObstacleSpeed;
		obstacle.spawnerParent = this;

		AddChild(obstacle);
		return obstacle;
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
