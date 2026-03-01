using Godot;
using System;

public partial class ObstacleSpawner : Marker2D
{
	[Export] public ObstaclePool[] ObstaclePools;
	[Export] public byte ObstacleLength = 1;
	[Export] public float ObstacleSpeed = 50f;
	[Export] public Vector2 SpawnDirection = Vector2.Left;
	[Export] public double SpawnInterval = 2.0f;
	[Export] public double ObstaclesMaxLifetime = 15.0f;

	private RandomNumberGenerator rand = new RandomNumberGenerator();
	private float[] randomWeights;
	private double spawnTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Define random weights for obstacle pool selection
		randomWeights = new float[ObstaclePools.Length];
		for (int i = 0; i < ObstaclePools.Length; i++)
			randomWeights[i] = GD.Randf();

		spawnTimer = SpawnInterval;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Update spawn timer
		spawnTimer -= delta;

		// Check if it's time to spawn a new obstacle
		if (spawnTimer <= 0)
		{
			// Pick a random obstacle pool
			ObstaclePool randomPool = ObstaclePools[rand.RandWeighted(randomWeights)];

			// Spawn a single obstacle
			if (ObstacleLength == 1)
			{
				Obstacle obstacle = SpawnObstacle(randomPool);

				switch (obstacle)
				{
					case Vehicle vehicle:
						vehicle.Initialize((VehicleType)GD.RandRange(1, Enum.GetNames(typeof(VehicleType)).Length));
						break;
				}
			}
			// Spawn multiple obstacles
			else
			{
				for (int i = 0; i < ObstacleLength; i++)
				{
					// Spawn obstacle with offset based on its position in the sequence
					Obstacle obstacle = SpawnObstacle(randomPool, Globals.TILE_SIZE * i);

					switch (obstacle)
					{
						case Log log:
							log.Initialize(ObstacleLength, i);
							break;

						case Crocodile:
							i = ObstacleLength; // Crocodiles may only have a length of 1
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
		obstacle.Direction = SpawnDirection;

		// Flip the obstacle if it's moving to the right
		// Note: Obstacles are designed to face left by default
		if (obstacle.Direction == Vector2.Right)
			obstacle.Scale = new Vector2(-1, 1);

		obstacle.Position += new Vector2(offset, 0);
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
