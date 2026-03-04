using Godot;
using System;

public partial class Lillypad : Area2D
{
    private Sprite2D frogSprite => GetNode<Sprite2D>("FrogSprite");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    // Check if player is on the lillypad
    private void _OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            frogSprite.Show();
            player.ReachLillypad();
        }
    }
}
