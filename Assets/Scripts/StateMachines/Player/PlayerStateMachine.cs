using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    // Creates a property with a public getter and a private setter. "[field: SerializeField]" allows us to see the property within the Inspector
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    // A reference to how much damage the weapon should do with each attack based on Damage values in the Attacks list
    [field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    // The movement speed of the player when in the free look state
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    // The movement speed of the player when targeting. Usually this is less than the free look movement speed
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
    // Relates to how fast the player will rotate to face the direction they are moving in
    [field: SerializeField] public float RotationDamping { get; private set; }
    // How long the dodge lasts in seconds
    [field: SerializeField] public float DodgeDuration { get; private set; }
    // How far to move the player when they dodge
    [field: SerializeField] public float DodgeLength { get; private set; }
    // Dodge Cooldown Timer. How many seconds the player must wait before being able to dodge again
    [field: SerializeField] public float DodgeCooldown { get; private set; }
    // How high the player will jump
    [field: SerializeField] public float JumpForce { get; private set; }
    // A list of Attacks. If there is more than one, previous Attacks combo into the next Attack in the list
    [field: SerializeField] public Attack[] Attacks { get; private set; }

    // Represents what Time.time was when the last dodge was performed. This seems like a weird way to do cooldowns
    // We set this to -Infinity to make sure there aren't any issues performing the very first dodge (when it would otherwise be 0)
    public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;
    public Transform MainCameraTransform { get; private set; }

    private void Start()
    {
        MainCameraTransform = Camera.main.transform;

        SwitchState(new PlayerFreeLookState(this));
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
        SwitchState(new PlayerImpactState(this));
    }

    private void HandleDie()
    {
        SwitchState(new PlayerDeadState(this));
    }

    // Why don't we just use the property declared above to set the value?
    public void SetDodgeTime(float dodgeTime)
    {
        PreviousDodgeTime = dodgeTime;
    }
}
