using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int AnimationHash = Animator.StringToHash("Fall");

    private Vector3 momentum;

    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // Remember player's momentum so they can still move on the xz-plane while falling
        momentum = stateMachine.Controller.velocity;
        // We don't care about the y-axis momentum because it's being handled by gravity while falling
        momentum.y = 0f;

        stateMachine.Animator.CrossFadeInFixedTime(AnimationHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        // This makes sure forces like gravity will always apply
        Move(momentum, deltaTime);

        if (stateMachine.Controller.isGrounded)
        {
            ReturnToLocomotion();
        }

        FaceTarget();
    }

    public override void Exit()
    {
    }
}
