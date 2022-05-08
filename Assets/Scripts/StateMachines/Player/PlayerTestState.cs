using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestState : PlayerBaseState
{
    [SerializeField] private float stateDuration = 5f;

    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter");
    }

    public override void Tick(float deltaTime)
    {
        stateDuration -= Time.deltaTime;
        Debug.Log(stateDuration);

        if (stateDuration <= 0f)
        {
            stateMachine.SwitchState(new PlayerTestState(stateMachine));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit");
    }
}
