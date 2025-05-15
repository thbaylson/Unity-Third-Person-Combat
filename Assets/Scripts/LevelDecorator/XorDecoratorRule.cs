using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[Serializable]
[CreateAssetMenu(fileName = "OrDecoratorRule", menuName = "ScriptableObjects/Procedural Generation/Or Decorator Rule")]
public class XorDecoratorRule : BaseDecoratorRule
{
    [SerializeField] BaseDecoratorRule[] childRules;
    
    private List<BaseDecoratorRule> availableRules = new();

    internal override void Apply(TileType[,] levelDecorated, Room room, Transform parent)
    {
        Random random = SharedLevelData.Instance.Rand;
        int ruleIndex = random.Next(availableRules.Count);
        availableRules[ruleIndex].Apply(levelDecorated, room, parent);
    }

    internal override bool CanBeApplied(TileType[,] levelDecorated, Room room)
    {
        foreach (BaseDecoratorRule rule in childRules)
        {
            if (rule.CanBeApplied(levelDecorated, room))
            {
                availableRules.Add(rule);
            }
        }

        return availableRules.Count > 0;
    }
}
