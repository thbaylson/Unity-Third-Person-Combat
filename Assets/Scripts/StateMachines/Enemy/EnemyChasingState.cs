using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChasingState : EnemyBaseState
{
    protected readonly int LocomotionHash = Animator.StringToHash("Locomotion");

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        // Can the NavMesh actually get to the player
        stateMachine.Agent.CalculatePath(stateMachine.Player.transform.position, stateMachine.Agent.path);
        bool navPathValid = stateMachine.Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete;
        //Debug.Log($"{stateMachine.transform.name}: Path Status: {stateMachine.Agent.pathStatus}; IsNavPathValid: {navPathValid}");
        //Debug.Log($"{stateMachine.transform.name}: Path Pending: {stateMachine.Agent.pathPending}; HasPath: {stateMachine.Agent.hasPath}; AgentDestination: {stateMachine.Agent.destination}; AgentPathEndPosition: {stateMachine.Agent.pathEndPosition}");

        // Is Desired Velocity the answer?
        stateMachine.Agent.destination = stateMachine.Player.transform.position;
        Debug.Log($"{stateMachine.transform.name}: Chase State Tick: Agent Destination: {stateMachine.Agent.destination}; Normalized Desired Velocity: {stateMachine.Agent.desiredVelocity.normalized}");

        // Player is out of range or inaccessible, so go back to idle
        if (!IsInChaseRange() || !navPathValid)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        //else if (IsInAttackRange() && navPathValid)
        //{
        //    stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
        //    return;
        //}
        FacePlayer();
        MoveToPlayer(deltaTime);

        stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    private void MoveToPlayer(float deltaTime)
    {
        Debug.Log($"{stateMachine.transform.name}: MoveToPlayer BEFORE: Agent Destination: {stateMachine.Agent.destination}; Normalized Desired Velocity: {stateMachine.Agent.desiredVelocity.normalized}");

        stateMachine.Agent.destination = stateMachine.Player.transform.position;
        Debug.Log($"{stateMachine.transform.name}: MoveToPlayer AFTER: Agent Destination: {stateMachine.Agent.destination}; Normalized Desired Velocity: {stateMachine.Agent.desiredVelocity.normalized}");
        Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

        //Debug.Log($"Before Velocity Update: Controller.velocity: {stateMachine.Controller.velocity}; Agent Velocity: {stateMachine.Agent.velocity};");
        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
        //Debug.Log($"After Velocity Update: Controller.velocity: {stateMachine.Controller.velocity}; Agent Velocity: {stateMachine.Agent.velocity};");
    }
}
