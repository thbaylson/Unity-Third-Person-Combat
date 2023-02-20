using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    public event Action OnDie;
    public event Action OnTakeDamage;

    public bool IsDead => health == 0;

    private int health;
    private bool isInvulnerable;

    // Start is called before the first frame update
    private void Start()
    {
        health = maxHealth;
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }

    public void DealDamage(int damage)
    {
        // Dead things don't take damage
        if(health == 0) { return; }

        // Invulnerable things don't take damage
        if (isInvulnerable) { return; }

        // If health is less than the damage being dealt, set health to 0
        // This prevents health from becoming negative
        health = health < damage ? 0 : health - damage;

        OnTakeDamage?.Invoke();

        if (health == 0)
        {
            OnDie?.Invoke();
        }
    }
}
