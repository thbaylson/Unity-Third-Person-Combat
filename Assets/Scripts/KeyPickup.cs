using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : Item
{
    float rotationSpeed = 0.15f;

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerInventory>().AddKey();
            Destroy(gameObject);
        }
    }
}
