using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingSquares : MonoBehaviour
{
    [SerializeField] Texture2D levelTexture;
    [SerializeField] GameObject generatedLevel;
    [SerializeField] Tileset tileset;

    [ContextMenu("Create Level Geometry")]
    public void CreateLevelGeometry()
    {
        generatedLevel.transform.DestroyAllChildren();

        int scale = SharedLevelData.Instance.Scale;
        Vector3 scaleVector = new Vector3(scale, scale, scale);

        TextureBasedLevel level = new TextureBasedLevel(levelTexture);
        for (int y = 0; y < level.Length - 1; y++)
        {
            for (int x = 0; x < level.Length - 1; x++)
            {
                int tileIndex = CalculateTileIndex(level, x, y);
                GameObject prefab = tileset.GetTile(tileIndex);
                if(prefab == null) continue;

                GameObject tile = Instantiate(prefab, generatedLevel.transform);
                tile.transform.localScale = scaleVector;
                tile.transform.position = new Vector3(x * scale, 0f, y * scale);
                tile.name = $"{x}, {y} - {tileIndex.ToString("00")}";
            }
        }
    }

    private int CalculateTileIndex(ILevel level, int x, int y)
    {
        // Calculate the tile index based on the state of the four corners
        // TODO: Consider a less hardcoded solution.
        int topLeft = level.IsBlocked(x, y + 1) ? 1 : 0;
        int topRight = level.IsBlocked(x + 1, y + 1) ? 1 : 0;
        int bottomLeft = level.IsBlocked(x, y) ? 1 : 0;
        int bottomRight = level.IsBlocked(x + 1, y) ? 1 : 0;
        int tileIndex = topLeft + 2*topRight + 4*bottomLeft + 8*bottomRight;

        return tileIndex;
    }
}
