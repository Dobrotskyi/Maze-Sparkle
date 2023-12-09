using System.Collections;
using UnityEngine;

public class ShotAgainAbility : Ability
{
    public override string Description => "Gives the ability to change direction of a shot";
    public override string Name => "Change direction";
    protected override Abilities _abilityType => Abilities.ShootAgain;

    protected override IEnumerator Use()
    {
        InvokeStarted();
        yield return new WaitForEndOfFrame();
        FindObjectOfType<Ball>().ShootAgain();
        yield return new WaitForEndOfFrame();
        InvokeFinished();
    }
}
