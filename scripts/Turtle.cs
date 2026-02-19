using Godot;
using System;

public partial class Turtle : Obstacle
{
	[Export] public float SubmergeDuration = 1.0f;
	[Export] public float SubmergeInterval = 2.0f;

	private CollisionShape2D collisionShape => GetNode<CollisionShape2D>("CollisionShape2D");
	private AnimatedSprite2D animatedSprite => GetNode<AnimatedSprite2D>("AnimatedSprite2D");

	private float submergeTimer;
	private bool isSubmerged = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		submergeTimer = SubmergeInterval;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);

		// Handle submerging behavior
		submergeTimer -= (float)delta;
		if (submergeTimer <= 0 && isSubmerged == false)
		{
			Submerge();
			submergeTimer = SubmergeDuration;
		}
		else if (submergeTimer <= 0 && isSubmerged == true)
		{
			Emerge();
			submergeTimer = SubmergeInterval;
		}
	}

	// Submerge the turtle, disabling its collision
	private void Submerge()
	{
		isSubmerged = true;
		collisionShape.Disabled = true;

		// Play submerge animation
		animatedSprite.Play("submerge");
	}

	// Emerge the turtle, enabling its collision
	private void Emerge()
	{
		isSubmerged = false;
		collisionShape.Disabled = false;

		// Play default swimming animation
		animatedSprite.Play("default");
	}

	// Check if player is on turtle
	private void _OnBodyEntered(Node2D body)
	{
		if (body is Player player)
		{
			player.platformCount++;
			player.currentPlatform = this;
			player.Velocity = Direction.Normalized() * Speed;
			GD.Print("Player moved onto turtle");
		}
	}

	// Check if player left turtle
	private void _OnBodyExited(Node2D body)
	{
		if (body is Player player)
		{
			player.platformCount--;
			if (player.currentPlatform == this)
			{
				player.currentPlatform = null;
			}
			GD.Print("Player left turtle");
		}
	}
}
