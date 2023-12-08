using System.Collections;
using UnityEngine;

public class AbilityFinger : Ability
{
    public static bool InUse { private set; get; }

    protected override Abilities _abilityType => Abilities.Finger;

    protected override IEnumerator Use()
    {
        if (AbilityIsUsed)
            yield break;

        InUse = true;
        AbilityIsUsed = true;
        Dragable selectedObject = null;
        Vector2 startObjectPosition = Vector2.zero;
        LaunchStarted();

        while (InUse)
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
                selectedObject = TouchInputs.GetObjectBehindFinger()?.GetComponent<Dragable>();
                if (selectedObject != null)
                {
                    startObjectPosition = selectedObject.transform.position;
                    LaunchStarted();
                }
            }

            if (selectedObject == null)
                goto End;

            selectedObject.DragTowards((Vector2)Camera.main.ScreenToWorldPoint(Input.touches[0].position));

            if (TouchInputs.TouchReleased())
            {
                InUse = false;
                AbilityIsUsed = false;
                selectedObject.PositionSet();
                selectedObject = null;
                LaunchFinished();
            }

        End:
            yield return new WaitForEndOfFrame();
        }
    }
}
