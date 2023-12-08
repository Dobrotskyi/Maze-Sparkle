using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{
    public static bool AbilityInUse { protected set; get; }
    public event Action Started;
    public event Action Finished;

    public int Price => PlayerInfoHolder.GetAbilityPrice(_abilityType);
    public int Amount => PlayerInfoHolder.AbilityAmount(_abilityType);
    public enum Abilities
    {
        Finger,
        Hammer
    }
    protected abstract Abilities _abilityType { get; }
    [SerializeField] private TextMeshProUGUI _amountField;
    [Header("Only in shop")]
    [SerializeField] private TextMeshProUGUI _priceField;
    private Button _button;

    protected abstract IEnumerator Use();
    public void UseAbility()
    {
        if (Amount == 0)
            return;
        if (!AbilityInUse)
            StartCoroutine(Use());
    }

    public void PurchaseAbility()
    {
        if (PlayerInfoHolder.TryPurchaseAbility(_abilityType))
            _amountField.text = Amount.ToString();
    }

    protected void AbilityUsed()
    {
        PlayerInfoHolder.AbilityUsed(_abilityType);
        _amountField.text = Amount.ToString();
        if (Amount == 0)
            _button.interactable = false;
    }

    protected void InvokeStarted()
    {
        Started?.Invoke();
    }

    protected void InvokeFinished()
    {
        Finished?.Invoke();
        AbilityUsed();
    }

    private void Awake()
    {
        _button = transform.GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        _amountField.text = Amount.ToString();
        if (_priceField != null)
            _priceField.text = Price.ToString();

        if (Amount == 0)
            _button.interactable = false;
    }
}
