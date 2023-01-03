using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event Action<Target> OnDestroyed;

    // Called by Unity when this GameObject is destroyed
    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}
