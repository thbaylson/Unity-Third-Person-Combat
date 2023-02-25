using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private readonly int AnimationHash = Animator.StringToHash("Jump");

    private Vector3 momentum;

    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);

        // Remember player's momentum so they can still move on the xz-plane while jumping
        momentum = stateMachine.Controller.velocity;
        // We don't care about the y-axis momentum because it's being handled by ForceReceiver.Jump()
        momentum.y = 0f;

        stateMachine.Animator.CrossFadeInFixedTime(AnimationHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        // This makes sure forces like gravity will always apply
        Move(momentum, deltaTime);

        // If we've reached the peak of our jump or if we're falling, then switch to the falling state
        if(stateMachine.Controller.velocity.y <= 0)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
        }

        FaceTarget();
    }

    public override void Exit()
    {
    }
}
