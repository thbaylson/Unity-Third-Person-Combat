using System;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class RoomTemplate
{
    [SerializeField] string name;

    [SerializeField] int roomCount;
    // TODO: Can I disable these properties in the inspector whenever a texture is added?
    [SerializeField] int roomWidthMin = 3;
    [SerializeField] int roomWidthMax = 5;
    [SerializeField] int roomLengthMin = 3;
    [SerializeField] int roomLengthMax = 5;
    [SerializeField] Texture2D layoutTexture;

    public string Name { get => name; }
    public int RoomCount { get => roomCount; }
    public int RoomWidthMin { get => roomWidthMin; }
    public int RoomWidthMax { get => roomWidthMax; }
    public int RoomLengthMin { get => roomLengthMin; }
    public int RoomLengthMax { get => roomLengthMax; }
    public Texture2D LayoutTexture { get => layoutTexture; }

    public RectInt GenerateRoomCandidateRect(Random random)
    {
        RectInt rect;
        if (layoutTexture != null)
        {
            rect = new RectInt { width = layoutTexture.width, height = layoutTexture.height };
        }
        else
        {
            rect = new RectInt { width = random.Next(roomWidthMin, roomWidthMax + 1), height = random.Next(roomLengthMin, roomLengthMax + 1) };
        }

        return rect;
    }
}