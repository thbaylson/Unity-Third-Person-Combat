using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class TileVariant
{
    [SerializeField] GameObject[] variants = new GameObject[0];
    
    public GameObject GetRandomTile()
    {
        Random random = SharedLevelData.Instance.Rand;
        int randomIndex = random.Next(variants.Length);

        return variants[randomIndex];
    }
}
