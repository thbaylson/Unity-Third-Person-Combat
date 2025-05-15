using UnityEngine;

// Currently, hallways are assumed to be 1 pixel wide and straight.
public class Hallway
{
    public Room StartRoom { get; set; }
    public Room EndRoom { get; set; }

    public Vector2Int StartPositionAbsolute => StartPosition + StartRoom.Area.position;
    public Vector2Int EndPositionAbsolute => EndPosition + EndRoom.Area.position;

    // Start direction will be known when the hallway is created and shouldn't change.
    public HallwayDirection StartDirection { get; private set; }
    // End direction will not be known until the connecting room is created, thus we need a setter.
    public HallwayDirection EndDirection { get; set; }
    
    public Vector2Int StartPosition { get; set; }
    public Vector2Int EndPosition { get; set; }

    public RectInt Area
    {
        get
        {
            int x = Mathf.Min(StartPositionAbsolute.x, EndPositionAbsolute.x);
            int y = Mathf.Min(StartPositionAbsolute.y, EndPositionAbsolute.y);
            int width = Mathf.Max(1, Mathf.Abs(StartPositionAbsolute.x - EndPositionAbsolute.x));
            int height = Mathf.Max(1, Mathf.Abs(StartPositionAbsolute.y - EndPositionAbsolute.y));

            // Hallways technically start and end inside rooms, but we want this property to only tell us the space outside of the rooms.
            // If the x coordinates are the same, then this is a vertical hallway.
            if (StartPositionAbsolute.x == EndPositionAbsolute.x)
            {
                y++; height--;
            }
            // If the y coordinates are the same, then this is a horizontal hallway.
            if (StartPositionAbsolute.y == EndPositionAbsolute.y)
            {
                x++; width--;
            }

            return new RectInt(x, y, width, height);
        }
    }

    // Note that a hallway will always overlap with the rooms it connects. For example, if there's a 3 pixel gap between the
    //  rooms, the hallway will be 5 pixels long.
    public Hallway(Vector2Int startPosition, HallwayDirection startDirection, Room startRoom= null)
    {
        StartPosition = startPosition;
        StartRoom = startRoom;

        StartDirection = startDirection;
    }
}
