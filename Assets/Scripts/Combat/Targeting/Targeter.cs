using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    public List<Target> targets = new List<Target>();

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
}
