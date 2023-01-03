using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        stateMachine.WeaponDamage.SetAttack(attack.Damage);

        // Transitions into the attack animation instead putting a hard stop to the current one
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        // Apply forces (eg. gravity) and face the target we want to attack
        Move(deltaTime);
        FaceTarget();

        float normalizedTime = GetNormalizedTime();
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

    /** Checking animation states can get weird. If we are mid transition, it's hard to tell which animation
     *  info we should be checking. This method will determine if we are transitioning and which info is needed. **/
    private float GetNormalizedTime()
    {
        // Gets the current and next AnimatorStateInfos at layer 0
        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

        float normalizedTime = 0f;

        // If we are currently transitioning animations AND the next animation is an attack animation...
        if(stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            normalizedTime = nextInfo.normalizedTime;
        }
        // If we are not currently transitioning animations AND the current animation is an attack animation...
        else if (!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            normalizedTime = currentInfo.normalizedTime;
        }

        // If neither of the above cases are true, this returns 0
        return normalizedTime;
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
