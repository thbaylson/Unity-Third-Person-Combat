using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace ThirdPersonCombat.RuntimeLevels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] List<Room> rooms = new List<Room>();
        [SerializeField] List<Room> roomPool = new List<Room>();
        [SerializeField] List<Type> placedItems = new List<Type>();

        [SerializeField] Door doorPrefab;
        [SerializeField] Door wallGapFiller;

        // Create an array of translations to perform on the current coordinate
        private static List<Vector2Int> translations = new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        int[][] roomArray;
        int numRooms;

        // The next room placed must be at least 2 rooms away from the last placed room to be considered backtracking
        int minBackTrackAmount = 2;
        int maxBackTrackAmount;
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

        public void SetRoomPool(List<Room> rooms)
        {
            roomPool = rooms;
        }

        public void SetBackTracking(int btFactor, int btChance)
        {
            maxBackTrackAmount = btFactor;
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
                currentRoom = AddRoom(GetNextRoomPosition());
                AddRoomContent(currentRoom);
            }

            AddDoors();
        }

        private void AddRoomContent(Room currentRoom)
        {
            //Add keys, enemies, items, etc
        }

        private Vector2Int GetNextRoomPosition()
        {
            // If we have no rooms, pick any random coordinate
            if (rooms.Count == 0) return GetRandomCoordinates();

            // Do we continue off of the last placed room, or backtrack to a previous room to continue off of?
            int backTrackAmount;
            if (rooms.Count > maxBackTrackAmount && !recentlyBackTracked)
            {
                bool isBackTracking = GetChance(backTrackChance);

                backTrackAmount = (isBackTracking) ? UnityEngine.Random.Range(minBackTrackAmount, maxBackTrackAmount) : 1;
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

            int cnt = 1;// Inifinite loop safegaurd. TODO: Find a better way
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

            // Get a list of rooms whose requirements are a subset of the list of items placed
            List<Room> roomPrefabs = GetValidRooms();

            // Get a random room from the list of prefabs and its Item if it has one
            Room roomPrefab = roomPrefabs[UnityEngine.Random.Range(0, roomPrefabs.Count)];
            placedItems.Add(roomPrefab.GetItem()?.GetType());

            // Keys are single use items. Placing a room that requires a key should remove the key from placedItems
            // Get a list of all the keys. This is important for counting keys
            List<Item> keys = roomPrefab.GetRequirements().Where(item => item.GetType() == typeof(KeyPickup)).ToList();
            if (keys.Count() > 0)
            {
                // Remove keys based on how many keys the last room placed required
                for(int i = 0; i < keys.Count(); i++)
                {
                    placedItems.Remove(typeof(KeyPickup));
                }
            }

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

        private List<Room> GetValidRooms()
        {
            List<Room> validRooms = new List<Room>();
            
            // If this is the very first room, then pick the first in the list. This will always make the start room safe
            // TODO: Make a SafeRoomPool to add variety and remove this hardcoded bit
            if(rooms.Count == 0)
            {
                validRooms.Add(roomPool[0]);
                return validRooms;
            }
            
            // Determine which rooms are valid
            foreach(Room r in roomPool)
            {
                List<Item> requirements = r.GetRequirements();

                // If a room does not have requirements, then it's always valid
                if(requirements.Count == 0)
                {
                    validRooms.Add(r);
                    continue;
                }

                bool allItemsPlaced = true;
                foreach(Item i in requirements)
                {
                    // Count how many times that item has been placed
                    int placedItemAmount = placedItems.Where(placed => placed == i.GetType())?.Count() ?? 0;

                    // Count how many of that item is required for this room
                    int requiredItemAmount = requirements.Where(requirement => requirement.GetType() == i.GetType())?.Count() ?? 0;

                    // If the Item has been placed at least as many times as it's required, then the room is valid
                    // Check for each item. If any of the items fail this condition, the room is not valid
                    allItemsPlaced = allItemsPlaced && (placedItemAmount >= requiredItemAmount);
                }

                if (allItemsPlaced)
                {
                    validRooms.Add(r);
                }
            }

            return validRooms;
        }

        private void AddDoors()
        {
            int checkX;
            int checkY;
            bool isRoom;
            bool isRoomNear;
            Vector2Int roomCoords;
            Room neighbor;

            foreach (Room r in rooms)
            {
                roomCoords = r.GetCoords();

                //For each cardinal direction, determine if there is an adjacent room
                // If there is, add a door
                foreach (Vector2Int translation in translations)
                {
                    checkX = roomCoords.x + translation.x;
                    checkY = roomCoords.y + translation.y;
                    isRoom = IsValidCoord(checkX, checkY) && roomArray[checkX][checkY] == 1;

                    neighbor = rooms.Find(r => r.GetCoords().Equals(new Vector2Int(checkX, checkY)));
                    isRoomNear = Mathf.Abs(rooms.IndexOf(r) - rooms.IndexOf(neighbor)) <= maxBackTrackAmount;

                    if (isRoom && isRoomNear)
                    {
                        r.SetDoor(doorPrefab, translation);
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