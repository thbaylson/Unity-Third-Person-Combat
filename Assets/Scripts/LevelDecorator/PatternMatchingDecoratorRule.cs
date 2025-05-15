using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[Serializable]
[CreateAssetMenu(fileName = "DecoratorRule", menuName = "ScriptableObjects/Procedural Generation/Pattern Decorator Rule")]
public class PatternMatchingDecoratorRule : BaseDecoratorRule
{
    [SerializeField] GameObject prefab;
    [SerializeField] float prefabRotation = 0f;

    [Tooltip("Make sure this has the same dimensions as fill.")]
    [SerializeField] Array2DWrapper<TileType> placement;
    [Tooltip("Make sure this has the same dimensions as placement.")]
    [SerializeField] Array2DWrapper<TileType> fill;

    [SerializeField] bool centerHorizontally = false;
    [SerializeField] bool centerVertically = false;

    internal override void Apply(TileType[,] levelDecorated, Room room, Transform parent)
    {
        Vector2Int[] occurrances = FindOccurrences(levelDecorated, room);
        if (occurrances.Length == 0) return;

        Random random = SharedLevelData.Instance.Rand;
        int occurranceIndex = random.Next(occurrances.Length);
        Vector2Int occurrance = occurrances[occurranceIndex];
            
        PlaceDecoration(levelDecorated, parent, occurrance);
    }

    private void PlaceDecoration(TileType[,] levelDecorated, Transform parent, Vector2Int occurrance)
    {
        if (prefab == null) return;

        for (int y = 0; y < placement.Height; y++)
        {
            for (int x = 0; x < placement.Width; x++)
            {
                TileType tileType = fill[x, y];
                // Don't place the tile if it's a No-Op
                if (TileType.Noop.Equals(tileType)) continue;

                levelDecorated[occurrance.x + x, occurrance.y + y] = tileType;
            }
        }

        GameObject decoration = Instantiate(prefab, parent.transform);
        Vector3 currentRotation = decoration.transform.rotation.eulerAngles;
        decoration.transform.eulerAngles = currentRotation + new Vector3(0f, prefabRotation, 0f);
        Vector3 center = new Vector3(occurrance.x + placement.Width / 2f, 0f, occurrance.y + placement.Height / 2f);
        int scale = SharedLevelData.Instance.Scale;
        decoration.transform.position = (center + new Vector3(-1, 0, -1)) * scale;
        decoration.transform.localScale = Vector3.one * scale;

        decoration.GetComponent<PropVariationGenerator>()?.GenerateVariation();
    }

    internal override bool CanBeApplied(TileType[,] levelDecorated, Room room)
    {
        return FindOccurrences(levelDecorated, room).Length > 0;
    }

    private Vector2Int[] FindOccurrences(TileType[,] levelDecorated, Room room)
    {
        List<Vector2Int> occurrences = new();
        int centerX = room.Area.position.x + (room.Area.width / 2) - (placement.Width / 2);
        int centerY = room.Area.position.y + (room.Area.height / 2) - (placement.Height / 2);

        // We need to iterate over each position in the room. Room coordinates do not include the walls,
        //  so we need to subtract 1 to start on the wall. This is also why we add 2 to the width and height of the room.
        //  We subtract placement.Width and Height to make sure the pattern doesn't go out of bounds. For example, we know
        //  that the top left corner of a 2 x 2 pattern can't start in the bottom right corner of the room.
        for (int y = room.Area.position.y - 1; y < room.Area.position.y + room.Area.height + 2 - placement.Height; y++)
        {
            for (int x = room.Area.position.x - 1; x < room.Area.position.x + room.Area.width + 2 - placement.Width; x++)
            {
                // Skip if the pattern is centered, but the current x,y is not in the center
                // TODO: Special room layouts with a hole in the center cannot place centered patterns, eg. the boss.
                if (centerHorizontally && x != centerX) continue;
                if (centerVertically && y != centerY) continue;

                if (IsPatternAtPosition(levelDecorated, placement, x, y))
                {
                    occurrences.Add(new Vector2Int(x, y));
                }
            }
        }

        return occurrences.ToArray();
    }

    private bool IsPatternAtPosition(TileType[,] levelDecorated, Array2DWrapper<TileType> pattern, int startX, int startY)
    {
        for(int yOffset = 0; yOffset < pattern.Height; yOffset++)
        {
            for (int xOffset = 0; xOffset < pattern.Width; xOffset++)
            {
                if (!TileType.Noop.Equals(pattern[xOffset, yOffset]) && levelDecorated[startX + xOffset, startY + yOffset] != pattern[xOffset, yOffset])
                {
                    return false;
                }
            }
        }

        return true;
    }
}
