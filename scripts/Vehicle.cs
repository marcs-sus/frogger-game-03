using Godot;
using System;

public partial class Vehicle : Obstacle
{
	private Sprite2D sprite => GetNode<Sprite2D>("Sprite2D");
	private string type = "Car1";

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

	// Initialize the vehicle with type and direction
	public void Initialize(string vehicleType, Vector2 direction)
	{
		type = vehicleType;
		Direction = direction.Normalized();
	}

	// Kill player if its body enters the vehicle area
	private void _OnBodyEntered(Node2D body)
	{
		if (body is Player player)
		{
			player.Die();
		}
	}
}
