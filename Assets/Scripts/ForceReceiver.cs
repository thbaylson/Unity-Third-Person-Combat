using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    // The player does not have a NavMeshAgent, only enemies do. This may need to be abstracted out in the future.
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float drag = 0.3f;
    // Used by SmoothDamp()
    private Vector3 dampingVelocity;
    private Vector3 impact;
    private float verticalVelocity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Update()
    {
        if (verticalVelocity < 0 && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        // This reduces forces over time using drag. It dampens impact towards zero.
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        // Because we are damping, the closer we get to zero the longer it'll take to actually hit zero.
        // So we define a threshold that's "close enough" before letting the agents move again.
        // If enemies look like they're running in place after getting hit, then change this threshold.
        float impactThreshold = 0.2f;
        bool isCloseEnough = impact.sqrMagnitude < impactThreshold * impactThreshold;
        if (agent != null && isCloseEnough)
        {
            impact = Vector3.zero;
            agent.enabled = true;
        }
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
        if(agent != null)
        {
            agent.enabled = false;
        }
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}
