using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] private float Speed = 200f;
	[Export] public Area2D WorldBorder;
	[Export] public Area2D WaterArea;

	private AnimatedSprite2D animatedSprite => GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	private Rect2 WorldBorderRect => WorldBorder.GetNode<CollisionShape2D>("CollisionShape2D").GetShape().GetRect();

	private Vector2 targetPosition;
	private bool isHopping = false;
	public Obstacle currentPlatform = null;
	public bool inWater = false;
	public short platformCount = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		targetPosition = Position;
	}

	// Called every physics frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		// Handle hopping movement
		if (isHopping)
		{
			// Move towards target position
			Position = Position.MoveToward(targetPosition, Speed * (float)delta);

			// Check if reached target position
			if (Position == targetPosition)
				isHopping = false;
		}
		// Handle movement on platforms
		else if (platformCount > 0 && currentPlatform != null)
		{
			// Move along with platform
			MoveAndSlide();
		}
		// Check for water death
		else if (inWater)
		{
			GD.Print("Died");
			Die();
		}

		// Check for out of bounds death
		if (!WorldBorderRect.HasPoint(Position))
		{
			GD.Print("Died");
			Die();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (isHopping) return;

		// Handle movement input
		if (@event.IsActionPressed("up"))
			TryMove(Vector2.Up);
		else if (@event.IsActionPressed("down"))
			TryMove(Vector2.Down);
		else if (@event.IsActionPressed("left"))
			TryMove(Vector2.Left);
		else if (@event.IsActionPressed("right"))
			TryMove(Vector2.Right);
	}

	private void TryMove(Vector2 direction)
	{
		// Calculate new target position
		targetPosition = Position + direction * Globals.TILE_SIZE;

		// If target isn't tile-aligned and not on a platform, snap to nearest tile
		if (platformCount == 0 && targetPosition.X % Globals.TILE_SIZE != 0)
			targetPosition.X = Mathf.Round(targetPosition.X / Globals.TILE_SIZE) * Globals.TILE_SIZE;

		// Do not move outside world border
		if (!WorldBorderRect.HasPoint(targetPosition))
			return;

		// Calculate rotation
		Rotation = direction.Angle() + Mathf.Pi / 2;

		// Play hop animation
		animatedSprite.Play("hop");

		isHopping = true;

		//GD.Print("New Target: " + targetPosition);
	}

	public void Die()
	{
		QueueFree(); // Temporary
	}
}
