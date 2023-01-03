using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    // Allows calls to the Animator to use this int hash instead of string references
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int BlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private const float AnimatorDampTime = 0.1f;
    // The transition time between this and another animation
    private const float CrossFadeDuration = 0.1f;
    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.Animator.CrossFadeInFixedTime(BlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        // Transition to attacking
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine));
            return;
        }

        // Use InputReader's MovementValue to get input information
        Vector3 movement = CalculateMovementDirection();

        // Pass motion information and deltaTime to PlayerBaseState
        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);


        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0f, AnimatorDampTime, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1f, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }

    private void OnTarget()
    {
        // If there are no available targets, do not enter the targeting state
        if (!stateMachine.Targeter.SelectTarget()) { return; }

        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
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

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping);
    }
}
