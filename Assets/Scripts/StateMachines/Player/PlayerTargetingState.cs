using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int BlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardSpeedHash = Animator.StringToHash("TargetingForwardSpeed");
    private readonly int TargetingRightSpeedHash = Animator.StringToHash("TargetingRightSpeed");

    private Vector2 dodgingDirectionInput;
    private float remainingDodgeTime;

    // Targeting State currently handles Targeting, Blocking, and Dodging. These things may be too tightly coupled
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.InputReader.CancelTargetEvent += OnCancelTarget;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;

        stateMachine.Animator.CrossFadeInFixedTime(BlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        // Transition to attacking
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine));
            return;
        }
        // Transition to blocking
        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }
        // Transition to free look
        if(stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement(deltaTime);

        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.CancelTargetEvent -= OnCancelTarget;
    }

    private void OnCancelTarget()
    {
        stateMachine.Targeter.Cancel();

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private void OnDodge()
    {
        // This seems like a really weird way to do cooldowns. Seems tightly coupled?
        // If the difference between the current time and the time when the last dodge was performed is less than the 
        //      dodge cooldown, then don't dodge. For example: if the difference is very small then we must've dodged 
        //      recently. The greater the difference, the greater amount of time must've passed.
        if (Time.time - stateMachine.PreviousDodgeTime < stateMachine.DodgeCooldown) { return; }

        // Store exactly which direction the player wants to dodge in
        dodgingDirectionInput = stateMachine.InputReader.MovementValue;
        remainingDodgeTime = stateMachine.DodgeDuration;

        stateMachine.SetDodgeTime(Time.time);
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
        return;
    }

    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();

        // If we are currently dodging
        if(remainingDodgeTime > 0f)
        {
            // The direction to move in
            var dodgeDirectionRight = stateMachine.transform.right * dodgingDirectionInput.x;
            var dodgeDirectionLeft = stateMachine.transform.forward * dodgingDirectionInput.y;
            // The distance to move during this Update (Remember: CalculateMovement will be called once per Update)
            var dodgeDistance = stateMachine.DodgeLength / stateMachine.DodgeDuration;
            
            movement += dodgeDirectionRight * dodgeDistance;
            movement += dodgeDirectionLeft * dodgeDistance;

            // Prevent remainingDodgeTime from becoming negative
            remainingDodgeTime = Mathf.Max(remainingDodgeTime - deltaTime, 0f);
        }
        // If we are not currently dodging
        else
        {
            movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
            movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;
        }

        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        float discreteMovementValue;

        if(stateMachine.InputReader.MovementValue.y == 0)
        {
            stateMachine.Animator.SetFloat(TargetingForwardSpeedHash, 0f, AnimatorDampTime, deltaTime);
        }
        else
        {
            discreteMovementValue = stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingForwardSpeedHash, discreteMovementValue, AnimatorDampTime, deltaTime);
        }

        if (stateMachine.InputReader.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(TargetingRightSpeedHash, 0f, AnimatorDampTime, deltaTime);
        }
        else
        {
            discreteMovementValue = stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingRightSpeedHash, discreteMovementValue, AnimatorDampTime, deltaTime);
        }
    }
}
