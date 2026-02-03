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
	public float platformSpeed = 0f;
	public Vector2 platformDirection = Vector2.Zero;
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
		else if (platformCount > 0)
		{
			// Move along with platform
			Position += platformDirection.Normalized() * platformSpeed * (float)delta;
		}
		// Check for water death
		else if (inWater)
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
		// Calculate new target position and rotation
		targetPosition = Position + direction * Globals.TILE_SIZE;

		// Do not move outside world border
		if (!WorldBorderRect.HasPoint(targetPosition))
			return;

		Rotation = direction.Angle() + Mathf.Pi / 2;
		//GD.Print("New Target: " + targetPosition);

		// Play hop animation
		animatedSprite.Play("hop");

		isHopping = true;
	}

	public void Die()
	{
		QueueFree(); // Temporary
	}
}
