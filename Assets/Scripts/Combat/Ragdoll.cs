using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private readonly string RagdollTagName = "Ragdoll";

    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;

    private Collider[] allColliders;
    private Rigidbody[] allRigidbodies;

    private void Start()
    {
        // This is an expensive operation. Try to use as sparingly as possible.
        allColliders = GetComponentsInChildren<Collider>(true);
        allRigidbodies = GetComponentsInChildren<Rigidbody>(true);

        // Turn everything off at the start.
        ToggleRagdoll(false);
        Debug.Log($"{this.name}: HERE");
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        foreach(Collider collider in allColliders)
        {
            if (collider.gameObject.CompareTag(RagdollTagName))
            {
                collider.enabled = isRagdoll;
            }
        }

        foreach (Rigidbody rigidbody in allRigidbodies)
        {
            if (rigidbody.gameObject.CompareTag(RagdollTagName))
            {
                // When isKinematic is turned off, the rigidbody will become affected by physics
                // If we want to ragdoll, we need to turn off isKinematic
                rigidbody.isKinematic = !isRagdoll;
                rigidbody.useGravity = isRagdoll;
            }
        }

        // If we want to ragdoll, we need to turn off the controller and animations
        controller.enabled = !isRagdoll;
        animator.enabled = !isRagdoll;
    }
}
