using System.Collections;
using TMPro;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public static bool AbilityIsUsed { protected set; get; }

    public int Amount => PlayerInfoHolder.AbilityAmount(_abilityType);
    public enum Abilities
    {
        Finger
    }
    protected abstract Abilities _abilityType { get; }
    [SerializeField] private TextMeshProUGUI _amountField;

    protected abstract IEnumerator Use();
    public void UseAbility()
    {
        //if (Amount == 0)
        //    return false;

        StartCoroutine(Use());
    }

    private void OnEnable()
    {
        _amountField.text = Amount.ToString();
    }
}
