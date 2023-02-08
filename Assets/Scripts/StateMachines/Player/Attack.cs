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
    // This represents a percentage of an animation, not an exact time interval
    [field: SerializeField] public float ComboAttackTime { get; private set; }
    // How far into the animation we need to be before we apply force to the character
    // This represents a percentage of how far into the attack animation we are (eg. 0 for just starting and 1 for just finished)
    [field: SerializeField] public float ForceTime { get; private set; }
    // The amount of force that gets added to the character
    [field: SerializeField] public float Force { get; private set; }
    // How much damage this attack will do
    [field: SerializeField] public int Damage { get; private set; }
    // How much knockback this attack will do
    [field: SerializeField] public float Knockback { get; private set; }

}
