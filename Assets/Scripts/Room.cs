using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum RoomType
{
    Default     = 1,
    Start       = 1 << 1,
    Exit        = 1 << 2,
    Boss        = 1 << 3,
    Treasure    = 1 << 4,
    Prison      = 1 << 5,
    Library     = 1 << 6,
    Shop        = 1 << 7,
}

public class Room
{
    public RectInt Area { get; private set; }
    public Texture2D LayoutTexture { get; }
    public RoomType Type { get; set; } = RoomType.Default;
    public int Connectedness => hallways.Count;

    private List<Hallway> hallways = new();

    public Room(RectInt area)
    {
        Area = area;
    }

    public Room(int x, int y, Texture2D layoutTexture)
    {
        Area = new RectInt(x, y, layoutTexture.width, layoutTexture.height);
        LayoutTexture = layoutTexture;
    }

    public List<Hallway> CalcPossibleDoorways(int width, int height, int minDistFromCorner)
    {
        List<Hallway> hallwayCandidates;
        if (LayoutTexture == null)
        {
            hallwayCandidates = CalcPossibleDoorwaysFromRect(width, height, minDistFromCorner);
        }
        else
        {
            hallwayCandidates = CalcPossibleDoorwaysFromTexture(LayoutTexture);
        }

        return hallwayCandidates;
    }

    public void AddHallway(Hallway hallway)
    {
        hallways.Add(hallway);
    }

    /// <summary>
    /// Helper function for CalcPossibleDoorways. Calculates all possible doorways for a room.
    /// The doorways are the walls of the room, minus the distance from the corners.
    /// </summary>
    /// <param name="width">The width of the room.</param>
    /// <param name="height">The height of the room.</param>
    /// <param name="minDistFromCorner">The minimum distance from the corner of the room.</param>
    /// <returns>A list of hallways. The start positions of the hallways will be relative to the room they are assigned to.</returns>
    private List<Hallway> CalcPossibleDoorwaysFromRect(int width, int height, int minDistFromCorner)
    {
        List<Hallway> hallwayCandidates = new List<Hallway>();

        int minX = minDistFromCorner;
        int maxX = width - minDistFromCorner;
        for(int x = minX; x < maxX; x++)
        {
            // Top, aka north wall
            hallwayCandidates.Add(new Hallway(new Vector2Int(x, height - 1), HallwayDirection.Top));
            // Bottom, aka south wall
            hallwayCandidates.Add(new Hallway(new Vector2Int(x, 0), HallwayDirection.Bottom));
        }

        int minY = minDistFromCorner;
        int maxY = height - minDistFromCorner;
        for (int y = minY; y < maxY; y++)
        {
            // Left, aka west wall
            hallwayCandidates.Add(new Hallway(new Vector2Int(0, y), HallwayDirection.Left));
            // Right, aka east wall
            hallwayCandidates.Add(new Hallway(new Vector2Int(width - 1, y), HallwayDirection.Right));
        }

        return hallwayCandidates;
    }

    private List<Hallway> CalcPossibleDoorwaysFromTexture(Texture2D layoutTexture)
    {
        List<Hallway> hallwayCandidates = new List<Hallway>();
        
        int width = layoutTexture.width;
        int height = layoutTexture.height;

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                Color pixelColor = layoutTexture.GetPixel(x, y);
                HallwayDirection direction = GetHallwayDirection(pixelColor);
                if (direction != HallwayDirection.Undefined)
                {
                    Hallway hallway = new Hallway(new Vector2Int(x, y), direction);
                    hallwayCandidates.Add(hallway);
                }
            }
        }

        return hallwayCandidates;
    }

    private HallwayDirection GetHallwayDirection(Color pixelColor)
    {
        var colorToDirectionMap = HallwayDirectionExtension.GetColorToDirectionMap();
        return colorToDirectionMap.TryGetValue(pixelColor, out HallwayDirection direction) ? direction : HallwayDirection.Undefined;
    }
}
