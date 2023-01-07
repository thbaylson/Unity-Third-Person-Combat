using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ThirdPersonCombat.RuntimeLevels
{
    public class LevelFactory : MonoBehaviour
    {
        [SerializeField] Level levelPrefab;
        [SerializeField] Level levelInstance;

        [SerializeField] GameObject playerPrefab;
        GameObject playerInstance;

        [SerializeField] int numRooms;
        [SerializeField] List<Room> roomPool;

        // backTrackFactor of less than 3 does nothing. Should probably never get close to 20, but I wanted a high ceiling just in case
        [SerializeField] [Range(3, 20)] int backTrackFactor;
        [SerializeField] [Range(0, 10)] int backTrackChance;
        [SerializeField] [Range(0, 10)] int changeDirectionChance;

        // Start is called before the first frame update
        void Start()
        {
            // If the level already exists, destroy it
            if (levelInstance != null) Destroy(levelInstance.gameObject);

            // Instantiate the level instance
            levelInstance = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity, transform);

            // Configure the level
            levelInstance.SetRoomNumber(numRooms);
            levelInstance.SetRoomPool(roomPool);
            levelInstance.SetBackTracking(backTrackFactor, backTrackChance);
            levelInstance.SetChangeDirectionChance(changeDirectionChance);
            levelInstance.Generate();

            // If the player already exists, destroy it
            if (playerInstance != null) Destroy(playerInstance.gameObject);

            // Get where the player should spawn
            Room initialRoom = levelInstance.GetRoomAt(0);
            Vector3 playerStartPos = new Vector3(initialRoom.transform.position.x, 0f, initialRoom.transform.position.z);
            
            // Instantiate the player instance
            playerInstance = Instantiate(playerPrefab, playerStartPos, Quaternion.identity, transform);

            // After everything is placed, bake the NavMesh
            this.GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }
}