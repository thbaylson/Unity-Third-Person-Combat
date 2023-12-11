using UnityEngine;

namespace ThirdPersonCombat.RuntimeLevels
{
    public class Door : MonoBehaviour, IDoor
    {
        private Animator stateAnimator;
        private bool _isClosed;

        public void SetClosedState(bool isClosed)
        {
            _isClosed = isClosed;
            if(stateAnimator != null)
            {
                stateAnimator.SetBool("isClosed", _isClosed);
            }
        }

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
                if (stateAnimator.Equals(null)) { return; }

                _isClosed = false;
                stateAnimator.SetBool("isClosed", _isClosed);
            }
        }
    }
}