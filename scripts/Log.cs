using Godot;
using System;

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
		Rect2 firstLogRegion = new Rect2(0, 0, Globals.TILE_SIZE, Globals.TILE_SIZE);
		Rect2 lastLogRegion = new Rect2(Globals.TILE_SIZE * 2, 0, Globals.TILE_SIZE, Globals.TILE_SIZE);

		if (lengthIndex == 0) // First log
			if (Direction == Vector2.Left)
				sprite.RegionRect = firstLogRegion;
			else
				sprite.RegionRect = lastLogRegion;
		else if (lengthIndex == logLength - 1) // Last log
			if (Direction == Vector2.Left)
				sprite.RegionRect = lastLogRegion;
			else
				sprite.RegionRect = firstLogRegion;
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
