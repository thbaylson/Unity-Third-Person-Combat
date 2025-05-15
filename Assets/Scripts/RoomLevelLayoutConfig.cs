using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room Level Layout", menuName = "ScriptableObjects/Procedural Generation/Room Level Layout Config", order = 1)]
public class RoomLevelLayoutConfig : ScriptableObject
{
    [Header("Level Layout Settings")]
    // Width and length should be powers of 2, otherwise behavior may be undefined.
    [SerializeField] int levelWidth = 64;
    [SerializeField] int levelLength = 64;
    [SerializeField] int levelPadding = 1;
    [SerializeField] int roomMargin = 1;

    [Header("Room Settings")]
    [SerializeField] int roomCountMin = 3;
    [SerializeField] int roomCountMax = 5;
    [SerializeField] RoomTemplate[] roomTemplates;
    [SerializeField] int doorwayDistanceFromCorner = 1;

    [Header("Hallway Settings")]
    [SerializeField] int hallwayWidthMin = 2;
    [SerializeField] int hallwayWidthMax = 3;

    public int LevelWidth { get => levelWidth; }
    public int LevelLength { get => levelLength; }
    public int LevelPadding { get => levelPadding; }
    public int RoomMargin { get => roomMargin; }
    public int RoomCountMin { get => roomCountMin; }
    public int RoomCountMax { get => roomCountMax; }
    public RoomTemplate[] RoomTemplates { get => roomTemplates; }
    public int DoorwayDistanceFromCorner { get => doorwayDistanceFromCorner; }
    public int HallwayWidthMin { get => hallwayWidthMin; }
    public int HallwayWidthMax { get => hallwayWidthMax; }

    public Dictionary<RoomTemplate, int> GetAvailableRooms()
    {
        Dictionary<RoomTemplate, int> availableRooms = new Dictionary<RoomTemplate, int>();
        for(int i = 0; i < roomTemplates.Length; i++)
        {
            int roomTemplateAmount = roomTemplates[i].RoomCount;
            if (roomTemplateAmount > 0)
            {
                availableRooms.Add(roomTemplates[i], roomTemplateAmount);
            }            
        }

        return availableRooms;
    }
}