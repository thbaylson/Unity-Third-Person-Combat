using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    public void SwitchState(State newState)
    {
        // Null Conditional Operator: The "?" symbol. Calls Exit() if currentState is not null. Does not work with MonoBehaviour or ScriptableObject classes.
        currentState?.Exit();
        currentState = newState;

        currentState?.Enter();
    }

    // Update is called once per frame
    private void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }
}
