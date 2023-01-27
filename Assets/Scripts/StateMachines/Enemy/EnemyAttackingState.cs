using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    protected readonly int AttackHash = Animator.StringToHash("Attack");

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // Consider a way to set range in the same way. Different weapons have different ranges.
        stateMachine.Weapon.SetAttack(stateMachine.AttackDamage);

        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}
