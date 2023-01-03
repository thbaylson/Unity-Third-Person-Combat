using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private List<Collider> alreadyCollidedWith = new();
    private int damage;

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure we don't hit ourselves
        if(other == myCollider) { return; }

        // Make sure we don't hit things multiple times per swing
        if (alreadyCollidedWith.Contains(other)) { return; }

        alreadyCollidedWith.Add(other);

        // Since we set the out param to Health, this implicitly tries to get a Health component
        if(other.TryGetComponent(out Health health))
        {
            health.DealDamage(damage);
        }
    }

    // Sets how much damage the weapon will do with each attack
    public void SetAttack(int damage)
    {
        this.damage = damage;
    }
}
