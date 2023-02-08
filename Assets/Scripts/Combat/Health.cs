using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int health;

    public event Action OnTakeDamage;

    // Start is called before the first frame update
    private void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(int damage)
    {
        // Dead things don't take damage
        if(health == 0) { return; }

        // If health is less than the damage being dealt, set health to 0
        // This prevents health from becoming negative
        health = health < damage ? 0 : health - damage;

        OnTakeDamage?.Invoke();
    }
}
