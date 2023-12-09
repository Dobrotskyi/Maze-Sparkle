using System.Collections;
using UnityEngine;

public class AbilityFinger : Ability, IInteractableAbility
{
    protected override Abilities _abilityType => Abilities.Finger;

    protected override IEnumerator Use()
    {
        Interactable selectedObject = null;
        Vector2 startObjectPosition = Vector2.zero;
        InvokeStarted();

        while (AbilityInUse)
        {
            if (TouchInputs.OverUINotClickthrough())
            {
                if (selectedObject != null)
                {
                    selectedObject.transform.position = startObjectPosition;
                    selectedObject.PositionSet();
                    selectedObject = null;
                }
                goto End;
            }

            if (TouchInputs.TouchBegan() && selectedObject == null)
            {
                selectedObject = TouchInputs.GetObjectBehindFinger()?.GetComponent<Interactable>();
                if (selectedObject != null)
                {
                    startObjectPosition = selectedObject.transform.position;
                    InvokeStarted();
                }
            }

            if (selectedObject == null)
                goto End;

            selectedObject.DragTowards((Vector2)Camera.main.ScreenToWorldPoint(Input.touches[0].position));

            if (TouchInputs.TouchReleased())
            {
                selectedObject.PositionSet();
                selectedObject = null;
                InvokeFinished();
            }

        End:
            yield return new WaitForEndOfFrame();
        }
    }
}
