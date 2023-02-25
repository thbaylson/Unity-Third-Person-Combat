public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;
    private bool alreadyAppliedForce;
    private Attack attack;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex= 0) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        // Get the attack damage for the current attack
        stateMachine.WeaponDamage.SetAttack(attack.Damage, attack.Knockback);

        // Transitions into the attack animation instead putting a hard stop to the current one
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        // Apply forces (eg. gravity) and face the target we want to attack
        Move(deltaTime);
        FaceTarget();

        float normalizedTime = GetNormalizedTime(stateMachine.Animator);
        // Check if we are still in the attack animation. NormalizedTime >= 1 means the animation has ended.
        if(normalizedTime < 1f)
        {
            // Apply Forces during the attack animation at just the right time
            if (normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }

            // Allow combos after the appropriate amount of time has passed
            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            // Leave attacking state. If still targeting, go to targeting state
            if(stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            // Leave attacking state. If not targeting, go to free look state
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }
        
        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
    }

    private void TryComboAttack(float normalizedTime)
    {
        // ComboStateIndex of -1 means that this attack does not combo
        if(attack.ComboStateIndex == -1) { return; }

        // If the normalizedTime is less than ComboAttackTime, then we are not yet ready to start the next attack
        if(normalizedTime < attack.ComboAttackTime) { return; }

        // If we've gotten this far, then we can switch states
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attack.ComboStateIndex));
    }

    private void TryApplyForce()
    {
        if(alreadyAppliedForce) { return; }

        // Apply attacking force as forward momentum
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);

        alreadyAppliedForce = true;
    }
}
