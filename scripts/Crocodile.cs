using Godot;
using System;

public partial class Crocodile : Obstacle
{
	[Export] public float OpenMouthDuration = 1.5f;
	[Export] public float OpenMouthInterval = 4.0f;

	private CollisionShape2D mouthCollisionShape => GetNode<Area2D>("MouthArea").GetNode<CollisionShape2D>("CollisionShape2D");
	private AnimatedSprite2D animatedSprite => GetNode<AnimatedSprite2D>("AnimatedSprite2D");

	private float openMouthTimer;
	private bool isMouthOpen = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		openMouthTimer = OpenMouthInterval;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);

		// Handle mouth opening and closing behavior
		openMouthTimer -= (float)delta;
		if (openMouthTimer <= 0 && isMouthOpen == false)
		{
			OpenMouth();
			openMouthTimer = OpenMouthDuration;
		}
		else if (openMouthTimer <= 0 && isMouthOpen == true)
		{
			CloseMouth();
			openMouthTimer = OpenMouthInterval;
		}
	}

	// Open the crocodile's mouth
	private void OpenMouth()
	{
		isMouthOpen = true;
		mouthCollisionShape.Disabled = false;

		// Play open mouth animation
		animatedSprite.Play("open_mouth");
	}

	// Close the crocodile's mouth
	private void CloseMouth()
	{
		isMouthOpen = false;
		mouthCollisionShape.Disabled = true;

		// Play close mouth animation
		animatedSprite.Play("closed_mouth");
	}

	// Check if player is on crocodile
	private void _OnBodyEntered(Node2D body)
	{
		if (body is Player player)
		{
			player.platformCount++;
			player.currentPlatform = this;
			player.Velocity = Direction.Normalized() * Speed;
			GD.Print("Player moved onto crocodile");
		}
	}

	// Check if player left crocodile
	private void _OnBodyExited(Node2D body)
	{
		if (body is Player player)
		{
			player.platformCount--;
			if (player.currentPlatform == this)
			{
				player.currentPlatform = null;
			}
			GD.Print("Player left crocodile");
		}
	}

	// Check if player stepped on crocodile's open mouth
	private void _OnBodyEnteredMouth(Node2D body)
	{
		if (body is Player player)
			player.Die();
	}
}
