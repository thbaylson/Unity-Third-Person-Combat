using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] CinemachineTargetGroup cineTargetGroup;

    private List<Target> targets = new List<Target>();
    public Target CurrentTarget { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Target enteringTarget)) { return; }

        // Add the object that has entered our collider to the list of targets
        targets.Add(enteringTarget);

        // Subscribe to the target's OnDestroyed
        enteringTarget.OnDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Target exitingTarget)) { return; }

        RemoveTarget(exitingTarget);
    }

    public bool SelectTarget()
    {
        // Return false because no target was set
        if(targets.Count == 0) { return false; }

        CurrentTarget = targets[0];
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        // Return true because we were able to set a target
        return true;
    }

    public void Cancel()
    {
        if (CurrentTarget == null) { return; }

        cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    /**<summary>
     * If the target to be removed is the CurrentTarget, then remove its transform from cinemachine's 
     * target group. Unsubscribe from the target's OnDestroy Action and remove the target from the
     * targets list.
     * </summary>*/
    private void RemoveTarget(Target target)
    {
        if(CurrentTarget == target)
        {
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}
