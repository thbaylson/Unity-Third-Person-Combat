using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float PlayerChaseRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public int AttackKnockback { get; private set; }

    public Health Player { get; private set; }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        // These default to true, but for this game we don't want Unity's NavMeshAgent to move enemies
        // Enemy movement will be handled by a combination of state machines and the ForceReceiver class
        Agent.updatePosition = false;
        Agent.updateRotation = false;

        SwitchState(new EnemyIdleState(this));
    }

    private void OnEnable()
    {
        // Subscribe to events
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        Health.OnDie -= HandleDie;
        Health.OnTakeDamage -= HandleTakeDamage;
    }

    private void HandleTakeDamage()
    {
        // Whenever we take damage, no matter what state we are currently in, switch to the impact state
        SwitchState(new EnemyImpactState(this));
    }

    private void HandleDie()
    {
        SwitchState(new EnemyDeadState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 180, 0);
        Gizmos.DrawWireSphere(transform.position, PlayerChaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
