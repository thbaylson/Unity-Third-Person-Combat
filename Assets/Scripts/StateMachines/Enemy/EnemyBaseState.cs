using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    // Allows calls to the Animator to use this int hash instead of string references
    protected readonly int SpeedHash = Animator.StringToHash("Speed");
    // This is how long the animator will take to get to the new value
    protected const float AnimatorDampTime = 0.1f;
    // The transition time between this and another animation
    protected const float CrossFadeDuration = 0.1f;

    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
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
        Debug.Log($"{stateMachine.transform.name}: Move: Motion: {motion}");

        // Use the CharacterController Component to move the player
        stateMachine.Controller.Move((stateMachine.ForceReceiver.Movement + motion) * deltaTime);
    }

    protected bool IsInChaseRange()
    {
        // Square magnitude of the distance between this enemy and the player
        float playerDistSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        // Because the magnitude is squared, we have to check that against the squared range
        // More computationally efficient to check squares than to take the square root
        return playerDistSqr <= stateMachine.PlayerChaseRange * stateMachine.PlayerChaseRange;
    }

    protected bool IsInAttackRange()
    {
        // Square magnitude of the distance between this enemy and the player
        float playerDistSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        // Because the magnitude is squared, we have to check that against the squared range
        // More computationally efficient to check squares than to take the square root
        return playerDistSqr <= stateMachine.AttackRange * stateMachine.PlayerChaseRange;
    }

    protected void FacePlayer()
    {
        if (stateMachine.Player == null) { return; }

        Vector3 lookAtVector = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookAtVector.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookAtVector);
    }
}
