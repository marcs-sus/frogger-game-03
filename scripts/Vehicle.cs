using Godot;
using System;
using System.Collections.Generic;

public partial class Vehicle : Area2D
{
	[Export] public float Speed = 50;

	private Sprite2D sprite => GetNode<Sprite2D>("Sprite2D");
	private string type = "Car1";
	private Vector2 direction = Vector2.Right;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Move the vehicle indefinitely
		Position = Position.Lerp(Position + direction * Speed * (float)delta, 1.0f);
	}

	// Initialize the vehicle with type and direction
	public void Initialize(string type, Vector2 direction)
	{
		this.type = type;
		this.direction = direction.Normalized();
	}
}
