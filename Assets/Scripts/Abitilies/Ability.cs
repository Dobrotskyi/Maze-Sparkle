using System.Collections;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    //public static bool AbilityIsUsed;

    public enum Abilities
    {
        Finger
    }

    public int Amount => PlayerInfoHolder.AbilityAmount(_abilityType);
    protected abstract Abilities _abilityType { get; }

    public bool TryUseAbility()
    {
        //if (Amount == 0)
        //    return false;

        StartCoroutine(Use());
        return true;
    }

    protected abstract IEnumerator Use();
}
