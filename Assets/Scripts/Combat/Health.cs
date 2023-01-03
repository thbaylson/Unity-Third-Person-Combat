using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int health;

    // Start is called before the first frame update
    private void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(int damage)
    {
        if(health == 0) { return; }

        // If health is less than the damage being dealt, set health to 0
        // This prevents health from becoming negative
        health = health < damage ? 0 : health - damage;
        Debug.Log($"{this.name} now has {health} health");
        // Alternative way to keep health >= 0: 
        //     health = Mathf.Max(health - damage, 0);
    }
}
