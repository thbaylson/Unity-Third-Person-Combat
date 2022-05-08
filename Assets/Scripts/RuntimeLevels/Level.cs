using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThirdPersonCombat.RuntimeLevels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] Room roomPrefab;
        [SerializeField] List<Room> rooms;

        [SerializeField] Door doorPrefab;
        [SerializeField] Door wallGapFiller;

        [SerializeField] KeyPickup keyPrefab;

        // Create an array of translations to perform on the current coordinate
        private static List<Vector2Int> translations = new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        int[][] roomArray;
        int numRooms;

        int keyDropChance;
        int numKeysPlaced = 0;

        int backTrackFactor;
        int backTrackChance;
        bool recentlyBackTracked;

        int changeDirectionChance;

        Vector2Int lastTranslation;

        public Room GetRoomAt(int index)
        {
            return rooms[index];
        }

        public void SetRoomNumber(int num)
        {
            numRooms = num;
        }

        public void SetKeyDropChance(int num)
        {
            keyDropChance = num;
        }

        public void SetBackTracking(int btFactor, int btChance)
        {
            backTrackFactor = btFactor;
            backTrackChance = btChance;
        }

        public void SetChangeDirectionChance(int chance)
        {
            changeDirectionChance = chance;
        }

        public void Generate()
        {
            MakeRoomArray();

            Room currentRoom;
            while (rooms.Count < numRooms)
            {
                // Make AddRoom return the Room object as room
                currentRoom = AddRoom(GetNextRoomPosition());
                AddRoomContent(currentRoom);
            }

            AddDoors();
        }

        private void AddRoomContent(Room currentRoom)
        {
            //Add keys, enemies, items, etc
            bool isKeyRoom = GetChance(keyDropChance);
            if (isKeyRoom)
            {
                currentRoom.Spawn(keyPrefab, transform.Find("Pickups"));
            }
        }

        private Vector2Int GetNextRoomPosition()
        {
            // If we have no rooms, pick any random coordinate
            if (rooms.Count == 0) return GetRandomCoordinates();

            // Do we continue off of the last placed room, or backtrack to a previous room to continue off of?
            int backTrackAmount;
            if (rooms.Count > 3 && !recentlyBackTracked)
            {
                bool isBackTracking = GetChance(backTrackChance);
                backTrackAmount = (isBackTracking) ? UnityEngine.Random.Range(1, backTrackFactor) : 1;
                recentlyBackTracked = backTrackAmount > 1;
            }
            else
            {
                backTrackAmount = 1;
                recentlyBackTracked = false;
            }

            // Get the room that we're going to continue off of
            Room room = rooms[rooms.Count - backTrackAmount];
            Vector2Int lastRoomCoords = room.GetCoords();

            // Continue off the last placed room
            Vector2Int newRoomCoords = GetNextNeighbor(lastRoomCoords);

            int cnt = 1;// Inifinite loop safegaurd. Find a better way
            bool roomFound = newRoomCoords != lastRoomCoords;
            while (!roomFound && (cnt++ <= 100))
            {
                newRoomCoords = GetNextNeighbor(lastRoomCoords);
                roomFound = newRoomCoords != lastRoomCoords;
                if (!roomFound)
                {
                    // Now lastRoomCoords is really "last room that hasn't had its neighbors checked"
                    lastRoomCoords = rooms[rooms.Count - cnt].GetCoords();
                }
            }

            if (cnt >= 100) print($"GetNextRoomPosition: Count: {cnt}");

            return newRoomCoords;
        }

        private bool GetChance(int chance)
        {
            return UnityEngine.Random.Range(0, 10) < chance;
        }

        /**<summary>Gets an open neighbor to the given coords OR if none exists returns the given coords</summary>*/
        private Vector2Int GetNextNeighbor(Vector2Int lastCoords)
        {
            // Shuffle the order of the translations array. Careful not to change the original array
            List<Vector2Int> tArray = translations.OrderBy(t => UnityEngine.Random.Range(0, translations.Count - 1)).ToList();

            // Keep going in the direction we were going, or pick a new direction?
            bool isChangingDirection = GetChance(changeDirectionChance);
            if (lastTranslation != null && !isChangingDirection)
            {
                Vector2Int temp = tArray[0];
                tArray[0] = lastTranslation;
                tArray.Add(temp);
            }

            int x;
            int y;
            Vector2Int tempCoord;
            Vector2Int newCoords = lastCoords;
            for (int i = 0; i < tArray.Count - 1; i++)
            {
                tempCoord = (lastCoords + tArray[i]);
                x = tempCoord.x;
                y = tempCoord.y;

                // Check coord validity
                if (IsValidCoord(x, y))
                {
                    // Check room is open
                    if (roomArray[x][y] == 0)
                    {
                        newCoords = new Vector2Int(x, y);
                        lastTranslation = tArray[i];
                        break;
                    }
                }
            }

            return newCoords;
        }

        private bool IsValidCoord(int x, int y)
        {
            return (x < roomArray.Length && x >= 0) && (y < roomArray.Length && y >= 0);
        }

        private Vector2Int GetRandomCoordinates()
        {
            int randX = UnityEngine.Random.Range(0, roomArray.Length);
            int randY = UnityEngine.Random.Range(0, roomArray.Length);
            return new Vector2Int(randX, randY);
        }

        private void MakeRoomArray()
        {
            // The length & width of the int[][]
            //int arrayLength = Mathf.CeilToInt(Mathf.Sqrt(numRooms));
            int arrayLength = numRooms;

            //int arrayLength = (UnityEngine.Random.Range(0, 2) == 0) ? numRooms : Mathf.CeilToInt(Mathf.Sqrt(numRooms));

            roomArray = new int[arrayLength][];
            for (int i = 0; i < roomArray.Length; i++)
            {
                roomArray[i] = new int[arrayLength];
            }
        }

        private Room AddRoom(Vector2Int position)
        {
            // "Fill" the index in the array
            roomArray[position.x][position.y] = 1;

            // Get the worldspace position
            Vector2 offset = new Vector2(position.x * roomPrefab.roomLength, position.y * roomPrefab.roomLength);
            Vector3 offsetPos = new Vector3(offset.x, 0f, offset.y);

            // Instantiate the Room
            Room roomInstance = Instantiate(roomPrefab, offsetPos, Quaternion.identity, transform.Find("Rooms"));
            roomInstance.SetCoords(position.x, position.y);
            roomInstance.SetTextInfo(rooms.Count);
            rooms.Add(roomInstance);

            // Return the newly created Room
            return roomInstance;
        }

        private void AddDoors()
        {
            int checkX;
            int checkY;
            Vector2Int roomCoords;

            foreach (Room r in rooms)
            {
                roomCoords = r.GetCoords();

                //For each cardinal direction, determine if there is an adjacent room
                // If there is, add a door
                foreach (Vector2Int translation in translations)
                {
                    checkX = roomCoords.x + translation.x;
                    checkY = roomCoords.y + translation.y;
                    if (IsValidCoord(checkX, checkY) && roomArray[checkX][checkY] == 1)
                    {
                        r.SetWall(doorPrefab, translation);
                    }
                    else
                    {
                        r.SetWall(wallGapFiller, translation);
                    }
                }
            }
        }
    }
}