using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] CinemachineTargetGroup cineTargetGroup;

    private Camera mainCamera;
    private List<Target> targets = new List<Target>();
    public Target CurrentTarget { get; private set; }

    private void Start()
    {
        mainCamera = Camera.main;
    }

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
        // Return false because there is nothing to target
        if(targets.Count == 0) { return false; }

        Target closestTarget = null;
        float closestTargetDist = Mathf.Infinity;
        foreach(Target target in targets)
        {
            // Get where the target is relative to the camera's viewport
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            // Only target if the enemy is actually visible. 
            // Putting a GetComponentInChildren check in a foreach loop seems inefficient. May need to be changed.
            if (!target.GetComponentInChildren<Renderer>().isVisible)
            {
                continue;
            }

            // Calculate how close this target is to the center of the screen, (0.5, 0.5)
            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);

            // Check to see if this target is closer to the center than the current closestTarget
            if(toCenter.sqrMagnitude < closestTargetDist)
            {
                // Square magnitude is more efficient to calc than just magnitude
                closestTarget = target;
                closestTargetDist = toCenter.sqrMagnitude;
            }
        }

        if(closestTarget == null) { return false; }

        CurrentTarget = closestTarget;
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
