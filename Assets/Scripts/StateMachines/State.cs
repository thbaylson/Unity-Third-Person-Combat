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
}
