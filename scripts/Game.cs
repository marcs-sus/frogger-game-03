using Godot;
using System;

public partial class Game : Node2D
{
	[Export] public ulong GameSeed = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set game seed
		GD.Seed(GameSeed);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
