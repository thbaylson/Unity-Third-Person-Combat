using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The class needs to be Serializable if you want to see it as a SerializedField property on a GameObject (eg. the Player)
[Serializable]
public class Attack
{
    // The name of the animation in the Animator
    [field: SerializeField] public string AnimationName { get; private set; }
    // The amount of time it should take for the previous animation to transition into the next one
    [field: SerializeField] public float TransitionDuration { get; private set; }
    // The index of the animation we will combo into. The next animation to play.
    // Default to -1 for attacks that do not combo or attacks that are the last attack of a combo
    [field: SerializeField] public int ComboStateIndex { get; private set; } = -1;
    // The minimum amount of time an animation must play before you can start the next attack
    // (This might represent a percentage of an animation, not an exact time interval)
    [field: SerializeField] public float ComboAttackTime { get; private set; }

}
