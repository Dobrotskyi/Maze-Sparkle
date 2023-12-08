using System.Collections;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public static bool AbilityIsUsed { protected set; get; }

    public enum Abilities
    {
        Finger
    }

    public int Amount => PlayerInfoHolder.AbilityAmount(_abilityType);
    protected abstract Abilities _abilityType { get; }

    public void UseAbility()
    {
        //if (Amount == 0)
        //    return false;

        StartCoroutine(Use());
    }

    protected abstract IEnumerator Use();
}
