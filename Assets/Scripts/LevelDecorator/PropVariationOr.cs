using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PropVariationOr : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private float probability = 0.5f;

    public void GenerateVariation()
    {
        Random random = SharedLevelData.Instance.Rand;
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(random.NextDouble() <= probability);
        }
    }
}
