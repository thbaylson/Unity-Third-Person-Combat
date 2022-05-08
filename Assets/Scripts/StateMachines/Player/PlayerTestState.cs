using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestState : PlayerBaseState
{
    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
    }

    public override void Tick(float deltaTime)
    {
        // Use InputReader's MovementValue to get input information
        Vector3 movement = CalculateMovementDirection();
        
        // Use the CharacterController Component to move the player
        stateMachine.Controller.Move(movement * stateMachine.FreeLookMovementSpeed * deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat("FreeLookSpeed", 0f, 0.1f, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat("FreeLookSpeed", 1f, 0.1f, deltaTime);
        stateMachine.transform.rotation = Quaternion.LookRotation(movement);
    }

    public override void Exit()
    {
    }

    /**<summary>Calculates the direction that the player will move in relative to the camera.</summary>*/
    private Vector3 CalculateMovementDirection()
    {
        // A normalized vector pointing forward from the camera. A vector that is pointing in the same direction that the camera is pointing.
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        // A normalized vector pointing right from the camera.
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // The sum of our forward direction multiplied by forward axis input and right direction multipled by right axis input
        return forward * stateMachine.InputReader.MovementValue.y +
            right * stateMachine.InputReader.MovementValue.x;
    }
}
