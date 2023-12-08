using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public enum Abilities
    {
        Finger
    }

    public int Amount => PlayerInfoHolder.AbilityAmount(_abilityType);
    protected abstract Abilities _abilityType { get; }

    public bool TryUseAbility()
    {
        if (Amount == 0)
            return false;

        Use();
        return true;
    }

    protected abstract void Use();
}
