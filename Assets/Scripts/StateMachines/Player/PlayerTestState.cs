using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestState : PlayerBaseState
{
    [SerializeField] private float timer;

    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter");

        // Subscribe to InputReader Action events
        stateMachine.InputReader.JumpEvent += OnJump;
    }

    public override void Tick(float deltaTime)
    {
        timer += Time.deltaTime;
        Debug.Log(timer);
    }

    public override void Exit()
    {
        Debug.Log("Exit");

        // Unsubscribe to InputReader Action events
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnJump()
    {
        Debug.Log("Jumping");
        stateMachine.SwitchState(new PlayerTestState(stateMachine));
    }
}
