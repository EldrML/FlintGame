using UnityEngine;

public static class Cardinal
{
    public const int SOUTH = 0;
    public const int SOUTHEAST = 1;
    public const int EAST = 2;
    public const int NORTHEAST = 3;
    public const int NORTH = 4;
    public const int NORTHWEST = 5;
    public const int WEST = 6;
    public const int SOUTHWEST = 7;

    public static int calculateFacingPositionForSpeed(Vector2 speed)
    {
        if (speed.x == 0)
        {
            if (speed.y < 0) return SOUTH;
            if (speed.y > 0) return NORTH;
        }
        if (speed.y == 0)
        {
            if (speed.x < 0) return WEST;
            if (speed.x > 0) return EAST;
        }
        if (speed.y > 0)
        {
            if (speed.x > 0) return NORTHEAST;
            if (speed.x < 0) return NORTHWEST;
        }
        if (speed.y < 0)
        {
            if (speed.x > 0) return SOUTHEAST;
            if (speed.x < 0) return SOUTHWEST;
        }

        return SOUTH;
    }
}