using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Ragdoll.ToggleRagdoll(true);
        
        // Disable the weapon object. We don't want potentially active hitboxes lying around.
        stateMachine.WeaponDamage.gameObject.SetActive(false);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}
