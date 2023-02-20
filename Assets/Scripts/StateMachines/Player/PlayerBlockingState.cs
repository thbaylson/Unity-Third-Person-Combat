using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private readonly int AnimationHash = Animator.StringToHash("Block");

    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // THIS IS GROSS! TODO: Find a way to make blocking less OP.
        stateMachine.Health.SetInvulnerable(true);
        stateMachine.Animator.CrossFadeInFixedTime(AnimationHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        // This makes sure forces like gravity will always apply
        Move(deltaTime);

        // If the player stops blocking, return to the targeting state
        if (!stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            return;
        }
        if(stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }
}
