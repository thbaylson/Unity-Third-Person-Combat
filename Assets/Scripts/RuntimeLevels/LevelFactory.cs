using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonCombat.RuntimeLevels
{
    public class LevelFactory : MonoBehaviour
    {
        [SerializeField] Level levelPrefab;
        [SerializeField] Level levelInstance;

        [SerializeField] PlayerStateMachine playerPrefab;
        PlayerStateMachine playerInstance;

        [SerializeField] CinemachineFreeLook freeLookCamera;

        [SerializeField] int numRooms;
        [SerializeField] [Range(0, 10)] int keyDropChance;

        [SerializeField] int backTrackFactor;
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
            levelInstance.SetKeyDropChance(keyDropChance);
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

            // Set the main camera's Follow and Look At fields
            freeLookCamera.Follow = playerInstance.transform.Find("CameraFocus");
            freeLookCamera.LookAt = playerInstance.transform.Find("mixamorig:Hips");
            
        }

        // Update is called once per frame
        void Update()
        {
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        if (levelInstance != null) Destroy(levelInstance.gameObject);

        //        levelInstance = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity, transform);

        //        levelInstance.SetRoomNumber(numRooms);
        //        levelInstance.SetKeyDropChance(keyDropChance);

        //        levelInstance.SetBackTracking(backTrackFactor, backTrackChance);
        //        levelInstance.SetChangeDirectionChance(changeDirectionChance);

        //        levelInstance.Generate();

        //        if (playerInstance != null) Destroy(playerInstance.gameObject);
        //        Room initialRoom = levelInstance.GetRoomAt(0);
        //        Vector3 playerStartPos = new Vector3(initialRoom.transform.position.x, playerPrefab.transform.localScale.y, initialRoom.transform.position.z);
        //        playerInstance = Instantiate(playerPrefab, playerStartPos, Quaternion.identity, transform);
        //    }
        }
    }
}