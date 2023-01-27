using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    protected readonly int LocomotionHash = Animator.StringToHash("Locomotion");

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        // TODO: Consider chasing IFF the enemy can "see" the player (e.g. shoot a raycast towards the player)
        if (IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
    }
}
