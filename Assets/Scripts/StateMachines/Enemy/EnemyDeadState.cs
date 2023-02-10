using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Ragdoll.ToggleRagdoll(true);

        // Disable the weapon object. We don't want potentially active hitboxes lying around.
        stateMachine.Weapon.gameObject.SetActive(false);

        // Prevent this enemy from being targetable
        Object.Destroy(stateMachine.Target);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}
