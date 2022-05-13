using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace ThirdPersonCombat.RuntimeLevels
{
    public class Door : MonoBehaviour
    {
        [SerializeField] TextMeshPro textMeshPro;
        [SerializeField] TextMeshPro textMeshPro2;
        [SerializeField] bool isLocked = true;

        // Doors need two colliders. One as a trigger and one as a non-trigger so players can't move through them
        BoxCollider doorCollider;

        // Awake is called before Start. Initialize properies here
        private void Awake()
        {
            // Grab the collider that prevents the player from moving through this object
            doorCollider = GetComponents<BoxCollider>().Where(c => !c.isTrigger).First();
        }

        void Update()
        {
            if (textMeshPro != null)
            {
                textMeshPro.text = (isLocked) ? "Locked Door" : "Open Door";
                textMeshPro2.text = (isLocked) ? "Locked Door" : "Open Door";
                doorCollider.enabled = isLocked;

                GetComponent<MeshRenderer>().enabled = isLocked;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.GetComponent<PlayerInventory>().HasKeys())
            {
                isLocked = false;
            }
        }
    }
}