using Godot;
using Godot.Collections;
//using System;

public enum Direction
{
	Right = 1,
	Left = -1,
}

public partial class ObstacleSpawner : Marker2D
{
	[Export] public Array<ObstaclePool> ObstaclePools;
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
			// Pick a random obstacle pool
			ObstaclePool randomPool = ObstaclePools.PickRandom();

			// Spawn a single obstacle
			if (ObstacleLength == 1)
			{
				SpawnObstacle(randomPool);
				spawnTimer = SpawnInterval;
			}
			else
			{
				// Spawn multiple obstacles
				for (int i = 0; i < ObstacleLength; i++)
				{
					// Spawn obstacle with offset based on its position in the sequence
					Obstacle obstacle = SpawnObstacle(randomPool, Globals.TILE_SIZE * i);

					switch (obstacle)
					{
						case Log log:
							// Change log sprite region based on the spawn order
							if (i == 0) // First log
								log.sprite.RegionRect = new Rect2(0, 0, Globals.TILE_SIZE, Globals.TILE_SIZE);
							else if (i == ObstacleLength - 1) // Last log
								log.sprite.RegionRect = new Rect2(Globals.TILE_SIZE * 2, 0, Globals.TILE_SIZE, Globals.TILE_SIZE);
							break;

						case Crocodile:
							// Crocodiles may only have a length of 1
							i = ObstacleLength;
							break;
					}
				}
			}

			spawnTimer = SpawnInterval * ObstacleLength;
		}
	}

	// Instantiate a new obstacle with the assigned scene and return it
	public Obstacle SpawnObstacle(ObstaclePool obstaclePool, float offset = 0f)
	{
		if (obstaclePool == null || obstaclePool.pool.Count <= 0)
		{
			GD.PrintErr("Obstacle pool is not assigned or empty.");
			return null;
		}

		Obstacle obstacle = obstaclePool.GetObstacle();
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
	public void RecycleObstacle(ObstaclePool obstaclePool, Obstacle obstacle)
	{
		if (obstaclePool != null)
		{
			RemoveChild(obstacle);
			obstaclePool.ReturnObstacle(obstacle);
		}
	}
}
