using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int AnimationHash = Animator.StringToHash("Impact");

    private float duration = 1f;

    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // Transitions into the impact animation instead putting a hard stop to the current one
        stateMachine.Animator.CrossFadeInFixedTime(AnimationHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        duration -= deltaTime;

        Move(deltaTime);

        if(duration <= 0f)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }
    }

    public override void Exit()
    {
    }
}
