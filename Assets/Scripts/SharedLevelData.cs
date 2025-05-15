using System;
using UnityEngine;
using Random = System.Random;

[ExecuteAlways]// This lets us use it while in Editor mode.
[DisallowMultipleComponent]// This ensures that the component is not added multiple times to the same GameObject.
public class SharedLevelData : MonoBehaviour
{
    [SerializeField] int scale = 1;
    [SerializeField] int seed = Environment.TickCount;
    public static SharedLevelData Instance { get; private set; }

    private Random random;

    public int Scale => scale;
    public Random Rand => random;

    public void ResetRandom()
    {
        random = new Random(seed);
    }

    [ContextMenu("Generate New Seed")]
    public void GenerateSeed()
    {
        seed = Environment.TickCount;
        ResetRandom();
    }

    private void OnEnable()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            // TODO: Destruction is more involved bc this script executes in editor mode.
            //  Lesson 3.7 (05:12) contains a high level description of the solution.
            enabled = false;
            Debug.LogWarning("Duplicate SharedLevelData detected and disabled.", this);
        }

        ResetRandom();
    }
}
