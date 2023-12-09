using System.Collections;
using UnityEngine;

public class AbilityHammer : Ability, IInteractableAbility
{
    protected override Abilities _abilityType => Abilities.Hammer;

    protected override IEnumerator Use()
    {
        InvokeStarted();
        Interactable selectedObject = null;
        while (AbilityInUse)
        {
            if (TouchInputs.TouchBegan())
                selectedObject = TouchInputs.GetObjectBehindFinger()?.GetComponent<Interactable>();

            if (selectedObject != null)
            {
                selectedObject.DestroySelf();
                InvokeFinished();
            }

            yield return new WaitForEndOfFrame();
        }
    }

}
