using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace ThirdPersonCombat.RuntimeLevels
{
    public class Door : MonoBehaviour
    {
        Animator stateAnimator;

        // Awake is called before Start. Initialize properies here
        private void Awake()
        {
            stateAnimator = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.GetComponent<PlayerInventory>().HasKeys())
            {
                // If animator isn't hooked up, return
                if (stateAnimator.Equals(null)) return;
                
                stateAnimator.SetBool("isClosed", false);
            }
        }
    }
}