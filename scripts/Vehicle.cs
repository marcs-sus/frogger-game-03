using Godot;
using System;
using static Globals;

public enum VehicleType
{
    Car = 1,
    SportsCar = 2,
    Truck = 3,
    Motorcycle = 4,
}

public partial class Vehicle : Obstacle
{
    private Sprite2D sprite => GetNode<Sprite2D>("Sprite2D");

    public VehicleType type = VehicleType.Car;

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

    // Initialize the vehicle with its type
    public void Initialize(VehicleType vehicleType)
    {
        type = vehicleType;
        switch (type)
        {
            case VehicleType.Car:
                sprite.RegionRect = CAR_REGION;
                break;
            case VehicleType.SportsCar:
                sprite.RegionRect = SPORTS_CAR_REGION;
                break;
            case VehicleType.Truck:
                sprite.RegionRect = TRUCK_REGION;
                break;
            case VehicleType.Motorcycle:
                sprite.RegionRect = MOTORCYCLE_REGION;
                break;
        }
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
