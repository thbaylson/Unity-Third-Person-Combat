using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    // Creates a property with a public getter and a private setter. "[field: SerializeField]" allows us to see the property within the Inspector
    [field: SerializeField] public InputReader InputReader { get; private set; }

    private void Start()
    {
        SwitchState(new PlayerTestState(this));
    }
}
