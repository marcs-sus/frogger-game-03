using Godot;
using System;

public partial class Turtle : Obstacle
{
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
