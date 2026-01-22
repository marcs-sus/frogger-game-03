using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] private float Speed = 200f;

	private AnimatedSprite2D animatedSprite => GetNode<AnimatedSprite2D>("AnimatedSprite2D");

	private const float TileSize = 32.0f;
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
			Position = Position.MoveToward(targetPosition, Speed * (float)delta);

			if (Position == targetPosition)
				isMoving = false;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (isMoving) return;

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
		targetPosition = Position + direction * TileSize;
		//GD.Print("New Target: " + targetPosition);
		animatedSprite.Play("hop");

		isMoving = true;
	}
}
