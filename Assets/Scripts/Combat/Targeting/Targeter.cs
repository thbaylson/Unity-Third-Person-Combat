using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    private List<Target> targets = new List<Target>();
    public Target CurrentTarget { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Target enteringTarget)) { return; }
        targets.Add(enteringTarget);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Target exitingTarget)) { return; }
        targets.Remove(exitingTarget);
    }

    public bool SelectTarget()
    {
        // Return false because no target was set
        if(targets.Count == 0) { return false; }

        CurrentTarget = targets[0];

        // Return true because we were able to set a target
        return true;
    }

    public void Cancel()
    {
        CurrentTarget = null;
    }
}
