using Godot;
using System;
using static Globals;

public partial class Ui : Control
{
    [Export] public Game GameNode;

    private Label scoreLabel => GetNode<Label>("ScoreLabel");
    private TextureRect livesCounter => GetNode<TextureRect>("LivesCounter");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        scoreLabel.Text = "SCORE: 00000";
        livesCounter.Size = new Vector2(TILE_SIZE * GameNode.PlayerLives, TILE_SIZE);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    // Update the score label
    public void UpdateScoreDisplay(int score)
    {
        scoreLabel.Text = "SCORE: " + score.ToString("D5");
    }

    // Update lives display counter
    public void UpdateLivesDisplay(int lives)
    {
        livesCounter.Size = new Vector2(TILE_SIZE * lives, TILE_SIZE);
    }
}
