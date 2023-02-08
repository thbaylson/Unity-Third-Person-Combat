using UnityEngine;

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
        bool navPathValid = stateMachine.Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete;

        // Player is out of range or inaccessible, so go back to idle
        if (!IsInChaseRange() || !navPathValid)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        else if (IsInAttackRange() && navPathValid)
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }

        FacePlayer();
        MoveToPlayer(deltaTime);

        stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        if (stateMachine.Agent.enabled)
        {
            stateMachine.Agent.ResetPath();
        }

        stateMachine.Agent.velocity = Vector3.zero;
    }

    private void MoveToPlayer(float deltaTime)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.destination = stateMachine.Player.transform.position;

            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }

        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }

    // TODO: This is nearly completely duplicated from IsInChaseRange(). Maybe those methods could be merged?
    protected bool IsInAttackRange()
    {
        // Square magnitude of the distance between this enemy and the player
        float playerDistSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        // Because the magnitude is squared, we have to check that against the squared range
        // More computationally efficient to check squares than to take the square root
        return playerDistSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
