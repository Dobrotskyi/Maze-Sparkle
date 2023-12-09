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
    public abstract string Description { get; }
    public abstract string Name { get; }
    public enum Abilities
    {
        Finger,
        Hammer,
        Teleportation,
        ShootAgain
    }
    protected abstract Abilities _abilityType { get; }
    [SerializeField] private TextMeshProUGUI _amountField;
    [Header("Only in shop")]
    [SerializeField] private TextMeshProUGUI _priceField;
    private Button _button;

    protected abstract IEnumerator Use();
    public void UseAbility()
    {
        //if (Amount == 0)
        //    return;
        if (!AbilityInUse)
            FindObjectOfType<AbilityUseDummy>().StartCoroutine(Use());
    }

    public void ShowInfo()
    {
        FindObjectOfType<SkillInfoPopUp>().ShowSkillInfo(Name, Description);
    }

    public void PurchaseAbility()
    {
        if (PlayerInfoHolder.TryPurchaseAbility(_abilityType))
            _amountField.text = Amount.ToString();
    }

    protected void InvokeStarted()
    {
        AbilityInUse = true;
        Started?.Invoke();
    }

    protected void InvokeCanceled()
    {
        StopAllCoroutines();
        AbilityInUse = false;
        Finished?.Invoke();
    }

    protected void InvokeFinished()
    {
        AbilityInUse = false;
        Finished?.Invoke();
        AbilityUsed();
    }

    protected void AbilityUsed()
    {
        //PlayerInfoHolder.AbilityUsed(_abilityType);
        _amountField.text = Amount.ToString();
        //if (Amount == 0)
        //    _button.interactable = false;
    }

    protected virtual void Awake()
    {
        _button = transform.GetComponentInChildren<Button>();
        InvokeCanceled();
        CancelUsageButton.AbilityCanceled += InvokeCanceled;
    }

    private void OnDestroy()
    {
        CancelUsageButton.AbilityCanceled -= InvokeCanceled;
    }

    private void OnEnable()
    {
        _amountField.text = Amount.ToString();
        if (_priceField != null)
            _priceField.text = Price.ToString();

        //if (Amount == 0 && _priceField == null)
        //    _button.interactable = false;
    }
}
