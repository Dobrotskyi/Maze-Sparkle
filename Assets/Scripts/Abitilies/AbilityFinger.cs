using System.Collections;
using UnityEngine;

public class AbilityFinger : Ability
{
    protected override Abilities _abilityType => Abilities.Finger;

    protected override IEnumerator Use()
    {
        bool abilityInUse = true;
        while (abilityInUse)
        {
            if (Input.touches.Length > 0)
            {

            }
            yield return new WaitForEndOfFrame();
        }
    }

}
