using UnityEngine;

public static class Cardinal
{
    public enum Direction { South, SouthEast, East, NorthEast, North, NorthWest, West, SouthWest }

    public static Direction CalculateFacingPositionForSpeed(Vector2 speed)
    {
        if (speed.x == 0)
        {
            if (speed.y < 0) return Direction.South;
            if (speed.y > 0) return Direction.North;
        }
        if (speed.y == 0)
        {
            if (speed.x < 0) return Direction.West;
            if (speed.x > 0) return Direction.East;
        }
        if (speed.y > 0)
        {
            if (speed.x > 0) return Direction.NorthEast;
            if (speed.x < 0) return Direction.NorthWest;
        }
        if (speed.y < 0)
        {
            if (speed.x > 0) return Direction.SouthEast;
            if (speed.x < 0) return Direction.SouthWest;
        }

        return Direction.South;
    }
}