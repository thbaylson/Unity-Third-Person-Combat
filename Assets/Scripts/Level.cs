using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int Width { get; private set; }
    public int Length { get; private set; }

    public Room[] Rooms => rooms.ToArray();
    public Hallway[] Hallways => hallways.ToArray();
    public Room PlayerStartRoom { get; set; }

    List<Room> rooms;
    List<Hallway> hallways;

    public Level(int width, int length)
    {
        Width = width;
        Length = length;

        rooms = new List<Room>();
        hallways = new List<Hallway>();
    }

    public void AddRoom(Room newRoom) => rooms.Add(newRoom);
    public void AddHallway(Hallway newHallway) => hallways.Add(newHallway);
}
