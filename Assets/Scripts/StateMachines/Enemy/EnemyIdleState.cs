using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    // Allows calls to the Animator to use this int hash instead of string references
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    // This is how long the animator will take to get to the new value
    private const float AnimatorDampTime = 0.1f;
    // The transition time between this and another animation
    private const float CrossFadeDuration = 0.1f;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
    }
}
