using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    // Called when entering a new state
    public abstract void Enter();
    // Called every frame to perform logic related to the current state
    public abstract void Tick(float deltaTime);
    // Called when exiting the current state
    public abstract void Exit();

    /** Checking animation states can get weird. If we are mid transition, it's hard to tell which animation
 *  info we should be checking. This method will determine if we are transitioning and which info is needed. **/
    protected float GetNormalizedTime(Animator animator)
    {
        // Gets the current and next AnimatorStateInfos at layer 0
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        float normalizedTime = 0f;

        // If we are currently transitioning animations AND the next animation is an attack animation...
        if (animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            normalizedTime = nextInfo.normalizedTime;
        }
        // If we are not currently transitioning animations AND the current animation is an attack animation...
        else if (!animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            normalizedTime = currentInfo.normalizedTime;
        }

        // If neither of the above cases are true, this returns 0
        return normalizedTime;
    }
}
