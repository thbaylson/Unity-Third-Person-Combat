using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;
    
    // This is how long the animator will take to get to the new value
    protected const float AnimatorDampTime = 0.1f;
    // The transition time between one animation and another
    protected const float CrossFadeDuration = 0.1f;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    // Overloaded Move method to update any forces being applied (eg. gravity, knockback, etc) without taking movement input
    // If we use some movement input, could we achieve a mechanic similar to SSBM's knockback DI?
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        // Use the CharacterController Component to move the player
        stateMachine.Controller.Move((stateMachine.ForceReceiver.Movement + motion) * deltaTime);
    }

    protected void FaceTarget()
    {
        if(stateMachine.Targeter.CurrentTarget == null) { return; }

        Vector3 lookAtVector = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        lookAtVector.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookAtVector);
    }

    // Helper method to be used in any state. Returns player to either FreeLookState or TargetingState
    protected void ReturnToLocomotion()
    {
        if(stateMachine.Targeter.CurrentTarget != null)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        else
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }
}
