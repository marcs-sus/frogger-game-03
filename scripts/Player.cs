using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] private float Speed = 200f;
	[Export] private float TileSize = 32.0f;
	[Export] public Area2D WorldBorder;

	private AnimatedSprite2D animatedSprite => GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	private Rect2 WorldBorderRect => WorldBorder.GetNode<CollisionShape2D>("CollisionShape2D").GetShape().GetRect();

	private Vector2 targetPosition;
	private bool isMoving = false;

	public override void _Ready()
	{
		targetPosition = Position;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (isMoving)
		{
			// Move towards target position
			Position = Position.MoveToward(targetPosition, Speed * (float)delta);

			// Check if reached target position
			if (Position == targetPosition)
				isMoving = false;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (isMoving) return;

		// Handle movement input
		if (@event.IsActionPressed("up"))
			Move(Vector2.Up);
		else if (@event.IsActionPressed("down"))
			Move(Vector2.Down);
		else if (@event.IsActionPressed("left"))
			Move(Vector2.Left);
		else if (@event.IsActionPressed("right"))
			Move(Vector2.Right);
	}

	private void Move(Vector2 direction)
	{
		// Calculate new target position and rotation
		targetPosition = Position + direction * TileSize;

		// Do not move outside world border
		if (WorldBorderRect.HasPoint(targetPosition) == false)
			return;

		Rotation = direction.Angle() + Mathf.Pi / 2;
		//GD.Print("New Target: " + targetPosition);

		// Play hop animation
		animatedSprite.Play("hop");

		isMoving = true;
	}
}
