using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class RuleAvailability
{
    public BaseDecoratorRule Rule;
    public int MaxAvailabilityPerRoom;
    public bool MustHave;

    // This constructor is used for copying the object.
    public RuleAvailability(RuleAvailability other)
    {
        Rule = other.Rule;
        MaxAvailabilityPerRoom = other.MaxAvailabilityPerRoom;
        MustHave = other.MustHave;
    }
}

public class RoomDecorator : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] RoomLayoutGenerator roomLayoutGenerator;
    [SerializeField] RuleAvailability[] availableRules;
    [SerializeField] Texture2D levelTexture;
    [SerializeField] Texture2D decoratedTexture;

    private Random random;

    [ContextMenu("Place Items")]
    public void PlaceItemsFromMenu()
    {
        SharedLevelData.Instance.ResetRandom();
        Level level = roomLayoutGenerator.GenerateLevel();
        PlaceItems(level);
    }

    public void PlaceItems(Level level)
    {
        random = SharedLevelData.Instance.Rand;
        Transform decorationsTransform = parent.transform.Find("Decorations");
        if (decorationsTransform == null)
        {
            decorationsTransform = new GameObject("Decorations").transform;
            decorationsTransform.SetParent(parent.transform);
        }
        else
        {
            decorationsTransform.DestroyAllChildren();
        }

        TileType[,] levelDecorated = InitializeDecoratorArray();
        foreach(Room room in level.Rooms)
        {
            DecorateRoom(levelDecorated, room, decorationsTransform);
        }
        GenerateTextureFromTileType(levelDecorated);
    }

    private TileType[,] InitializeDecoratorArray()
    {
        TileType[,] levelDecorated = new TileType[levelTexture.width, levelTexture.height];
        for (int y = 0; y < levelTexture.height; y++)
        {
            for (int x = 0; x < levelTexture.width; x++)
            {
                Color pixelColor = levelTexture.GetPixel(x, y);
                if (pixelColor == Color.black)
                {
                    levelDecorated[x, y] = TileType.Wall;
                }
                else
                {
                    levelDecorated[x, y] = TileType.Floor;
                }
            }
        }

        return levelDecorated;
    }

    private void DecorateRoom(TileType[,] levelDecorated, Room room, Transform decorationsTransform)
    {
        int maxTries = 50;
        int currentTries = 0;

        // TODO: Make max num decorations configurable.
        int maxNumDecorations = (int)(room.Area.width * room.Area.height * 0.15f);
        int numDecorationsToPlace = random.Next(maxNumDecorations);
        int currentDecorations = 0;

        // Copy all the rules available for the level (since we modify the array), and then filter them based on the room type.
        List<RuleAvailability> availableRulesForRoom = CopyRuleAvailability()
            .Where(ra => ra.Rule.RoomTypes.HasFlag(room.Type))
            .Where(ra => ra.MaxAvailabilityPerRoom != 0)
            .ToList();

        // Shuffle the available rooms.
        availableRulesForRoom = availableRulesForRoom.OrderBy(x => random.Next()).ToList();

        // First place the decorations that we absolutely must have.
        List<RuleAvailability> mustHaves = availableRulesForRoom.Where(ra => ra.MustHave).ToList();
        foreach (RuleAvailability mustHave in mustHaves)
        {
            if (mustHave.Rule.CanBeApplied(levelDecorated, room))
            {
                mustHave.Rule.Apply(levelDecorated, room, decorationsTransform);
            }
            availableRulesForRoom.Remove(mustHave);
        }

        while (currentDecorations < maxNumDecorations && currentTries < maxTries && availableRulesForRoom.Count > 0)
        {
            int selectedRuleIndex = random.Next(availableRulesForRoom.Count);
            RuleAvailability selectedRuleAvailability = availableRulesForRoom[selectedRuleIndex];
            BaseDecoratorRule selectedRule = selectedRuleAvailability.Rule;

            if (selectedRule.CanBeApplied(levelDecorated, room))
            {
                selectedRule.Apply(levelDecorated, room, decorationsTransform);
                currentDecorations++;
                
                // As a consequence of this check, MaxAvailability of -1 means unlimited availability.
                if (selectedRuleAvailability.MaxAvailabilityPerRoom > 0)
                {
                    selectedRuleAvailability.MaxAvailabilityPerRoom--;
                }
                if (selectedRuleAvailability.MaxAvailabilityPerRoom == 0)
                {
                    availableRulesForRoom.Remove(selectedRuleAvailability);
                }
            }

            currentTries++;
        }
    }

    private void GenerateTextureFromTileType(TileType[,] tileTypes)
    {
        int width = tileTypes.GetLength(0);
        int height = tileTypes.GetLength(1);

        Color32[] pixels = new Color32[width * height];
        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                pixels[x + y * width] = tileTypes[x, y].GetColor();
            }
        }

        decoratedTexture.Reinitialize(width, height);
        decoratedTexture.SetPixels32(pixels);
        decoratedTexture.Apply();
        decoratedTexture.SaveAsset();
    }

    private List<RuleAvailability> CopyRuleAvailability()
    {
        List<RuleAvailability> availableRulesForRoom = new();
        availableRules.ToList().ForEach(rule => availableRulesForRoom.Add(new RuleAvailability(rule)));
        return availableRulesForRoom;
    }
}
