using System;
using UnityEngine;
using UnityEngine.UI;

public class CancelUsageButton : MonoBehaviour
{
    public static event Action AbilityCanceled;
    public void CancelUsage()
    {
        AbilityCanceled?.Invoke();
    }

    private void FixedUpdate()
    {
        if (gameObject.activeSelf && !Ability.AbilityInUse)
            GetComponent<Button>().onClick.Invoke();
    }
}
