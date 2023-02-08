using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    private readonly int AnimationHash = Animator.StringToHash("Impact");

    private float duration = 1f;

    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // Transitions into the impact animation instead putting a hard stop to the current one
        stateMachine.Animator.CrossFadeInFixedTime(AnimationHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        duration -= deltaTime;

        Move(deltaTime);

        if (duration <= 0f)
        {
            // If we were targeting before getting hit, we want to return to the targeting state
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
    }
}
