using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ThirdPersonCombat.RuntimeLevels
{
    public class Room : MonoBehaviour
    {
        public float roomLength;

        [SerializeField] TextMeshPro textInfo;
        [SerializeField] Transform northWallGap;
        [SerializeField] Transform eastWallGap;
        [SerializeField] Transform southWallGap;
        [SerializeField] Transform westWallGap;

        private Vector2Int coords;
        private List<Item> progressionRequirements = new List<Item>();

        public void AddRequirement(Item item)
        {
            progressionRequirements.Add(item);
        }

        public void Spawn(Item item, Transform parent)
        {
            Vector3 position = transform.position + new Vector3(0f, item.transform.localScale.y, 0f);
            Item itemInstance = Instantiate(item, position, Quaternion.identity, parent);
        }

        public Vector2Int GetCoords()
        {
            return coords;
        }

        public void SetTextInfo(int number)
        {
            textInfo.text = $"{number}\n({coords.x},{coords.y})";
        }

        public void SetCoords(int x, int y)
        {
            coords = new Vector2Int(x, y);
        }
        public void SetWall(Door gapFiller, Vector2Int translation)
        {
            if (translation.Equals(Vector2Int.up))
            {
                SetNorthWall(gapFiller);
            }
            else if (translation.Equals(Vector2Int.down))
            {
                SetSouthWall(gapFiller);
            }
            else if (translation.Equals(Vector2Int.left))
            {
                SetWestWall(gapFiller);
            }
            else if (translation.Equals(Vector2Int.right))
            {
                SetEastWall(gapFiller);
            }
        }

        private void SetNorthWall(Door gapFiller)
        {
            SetWall(northWallGap, gapFiller);
        }

        private void SetEastWall(Door gapFiller)
        {
            Door door = SetWall(eastWallGap, gapFiller);
            door.transform.localRotation = new Quaternion(0f, 90f, 0f, 0f);
        }

        private void SetSouthWall(Door gapFiller)
        {
            Door door = SetWall(southWallGap, gapFiller);
            door.transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
        }

        private void SetWestWall(Door gapFiller)
        {
            Door door = SetWall(westWallGap, gapFiller);
            door.transform.localRotation = new Quaternion(0f, 270f, 0f, 0f);
        }

        private Door SetWall(Transform wallGap, Door gapFiller)
        {
            // If there's already a door there, replace it
            BoxCollider previousFiller = wallGap.GetComponentInChildren<BoxCollider>();
            // Make sure we don't delete the wallGap transform itself
            if (previousFiller != null) Destroy(previousFiller.gameObject);

            Door door = Instantiate(gapFiller, Vector3.zero, Quaternion.identity, wallGap);
            door.transform.localPosition = Vector3.zero;
            return door;
        }
    }
}