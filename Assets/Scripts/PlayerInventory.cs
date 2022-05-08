using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] int keys = 0;

    public void AddKey()
    {
        keys++;
    }

    public void RemoveKey()
    {
        keys--;
    }

    public bool HasKeys()
    {
        return keys > 0;
    }
}
