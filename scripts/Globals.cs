using Godot;

public static class Globals
{
    // General constants
    public const float TILE_SIZE = 32.0f;

    // Log constants
    public static readonly Rect2 FIRST_LOG_REGION = new Rect2(0.0f, 0.0f, TILE_SIZE, TILE_SIZE);
    public static readonly Rect2 LAST_LOG_REGION = new Rect2(TILE_SIZE * 2.0f, 0.0f, TILE_SIZE, TILE_SIZE);

    // Vehicle constants
    public static readonly Rect2 CAR_REGION = new Rect2(0.0f, 0.0f, TILE_SIZE, TILE_SIZE);
    public static readonly Rect2 SPORTS_CAR_REGION = new Rect2(TILE_SIZE, 0.0f, TILE_SIZE, TILE_SIZE);
    public static readonly Rect2 TRUCK_REGION = new Rect2(0.0f, TILE_SIZE, TILE_SIZE, TILE_SIZE);
    public static readonly Rect2 MOTORCYCLE_REGION = new Rect2(TILE_SIZE, TILE_SIZE, TILE_SIZE, TILE_SIZE);
}
