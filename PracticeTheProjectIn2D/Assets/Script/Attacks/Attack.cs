using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "New Attack")]
public class Attack : ScriptableObject
{
    public int damage;
    public AnimationClip animation;

    public string triggerName;

    public KeyCode requiredInputKey;

    public float attackRange;

    public Attack nextAttack;

    public Attack getNextCombo()
    {
        Debug.Log("Attack " + triggerName);
        return nextAttack;
    }

    public string getTriggerName()
    {
        return triggerName;
    }

    public KnockbackDirection knockback;

    public enum KnockbackDirection
    {
        KnockUp,
        KnockBack,
        Pull
    }
}




