using UnityEngine;
using UnityEngine.AI;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] RoomLayoutGenerator roomLayoutGenerator;
    [SerializeField] MarchingSquares levelGeometryGenerator;
    [SerializeField] NavMeshSurface navMeshSurface;
    [SerializeField] RoomDecorator roomDecorator;
    [SerializeField] GameObject playerInstance;

    [ContextMenu("Gen Level And Geometry")]
    public void GenerateLayoutAndGeometry()
    {
        roomLayoutGenerator.GenerateNewSeed();
        Level level = roomLayoutGenerator.GenerateLevel();
        levelGeometryGenerator.CreateLevelGeometry();
        roomDecorator.PlaceItems(level);
        navMeshSurface.BuildNavMesh();

        Room startRoom = level.PlayerStartRoom;
        Vector3 startPosition = LevelPositionToWorldPosition(startRoom.Area.center);

        // Why not make the player a property of LevelBuilder and instantiate it?
        playerInstance.transform.position = startPosition;
    }

    private Vector3 LevelPositionToWorldPosition(Vector2 levelPosition)
    {
        int scale = SharedLevelData.Instance.Scale;
        return new Vector3(
            (levelPosition.x - 1) * scale,
            0f,
            (levelPosition.y - 1) * scale
        );
    }

    private void Start()
    {
        //GenerateLayoutAndGeometry();
    }
}
