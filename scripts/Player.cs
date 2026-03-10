using Godot;
using System;
using static Globals;

public partial class Player : CharacterBody2D
{
    [Export] private float Speed = 200f;
    [Export] private double RespawnDuration = 2.0;
    [Export] public Game GameNode;
    [Export] public Area2D WorldBorder;
    [Export] public Area2D WaterArea;

    private AnimatedSprite2D animatedSprite => GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    private Rect2 WorldBorderRect => WorldBorder.GetNode<CollisionShape2D>("CollisionShape2D").GetShape().GetRect();
    private CpuParticles2D deathParticles => GetNode<CpuParticles2D>("CPUParticles2D");

    private Vector2 targetPosition;
    private bool isHopping = false;
    private bool isRespawning = false;
    public bool inWater = false;
    public Obstacle currentPlatform = null;
    public short platformCount = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        targetPosition = Position;
        deathParticles.Lifetime = RespawnDuration;
    }

    // Called every physics frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        // Handle respawning
        if (isRespawning)
        {
            // Wait for respawn duration before respawning
            RespawnDuration -= delta;
            if (RespawnDuration <= 0)
                Respawn();

            return;
        }

        // Handle hopping movement
        if (isHopping)
        {
            // Move towards target position
            Position = Position.MoveToward(targetPosition, Speed * (float)delta);

            // Check if reached target position
            if (Position == targetPosition)
                isHopping = false;
        }
        // Handle movement on platforms
        else if (platformCount > 0 && currentPlatform != null)
        {
            // Move along with platform
            MoveAndSlide();
        }
        // Check for water death
        else if (inWater)
        {
            GD.Print("Died");
            Die();
        }

        // Check for out of bounds death
        if (!WorldBorderRect.HasPoint(Position))
        {
            GD.Print("Died");
            Die();
        }
    }

    // Called when a input event is received.
    public override void _Input(InputEvent @event)
    {
        if (isRespawning || isHopping) return;

        // Handle movement input
        if (@event.IsActionPressed("up"))
            TryMove(Vector2.Up);
        else if (@event.IsActionPressed("down"))
            TryMove(Vector2.Down);
        else if (@event.IsActionPressed("left"))
            TryMove(Vector2.Left);
        else if (@event.IsActionPressed("right"))
            TryMove(Vector2.Right);
    }

    // Attempt to move the player in the given direction
    private void TryMove(Vector2 direction)
    {
        if (isRespawning) return;

        // Calculate new target position
        targetPosition = Position + direction * TILE_SIZE;

        // If target isn't tile-aligned and not on a platform, snap to nearest tile
        if (platformCount == 0 && targetPosition.X % TILE_SIZE != 0)
            targetPosition.X = Mathf.Round(targetPosition.X / TILE_SIZE) * TILE_SIZE;

        // Do not move outside world border
        if (!WorldBorderRect.HasPoint(targetPosition))
            return;

        // Calculate rotation
        Rotation = direction.Angle() + Mathf.Pi / 2;

        // Play hop animation
        animatedSprite.Play("hop");

        isHopping = true;

        //GD.Print("New Target: " + targetPosition);
    }

    // Handle player death
    public void Die()
    {
        // Subtract player lives counter
        GameNode.UpdateLives();

        // Emit death particles
        deathParticles.Emitting = true;

        isRespawning = true;
        DisablePlayer();
    }

    // Handle reaching a lillypad
    public void ReachLillypad()
    {
        isRespawning = true;
        DisablePlayer();
    }

    // Hide and disable player
    private void DisablePlayer()
    {
        animatedSprite.Hide();
        isHopping = false;
        inWater = false;
        platformCount = 0;
        currentPlatform = null;
    }

    // Reset player states and position for respawn
    public void Respawn()
    {
        // Reset death state and duration
        isRespawning = false;
        RespawnDuration = 2.0;

        // Reset positions and rotation
        Position = GameNode.PlayerStartPosition;
        Rotation = 0;
        targetPosition = Position;

        // Show player
        animatedSprite.Show();
    }
}
