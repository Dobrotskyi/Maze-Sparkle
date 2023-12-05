using TMPro;
using UnityEngine;

[RequireComponent(typeof(CountingAnimation))]
public class CoinsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amtField;
    private CountingAnimation _countAnim;

    private void Awake()
    {
        _countAnim = GetComponent<CountingAnimation>();
    }

    private void OnEnable()
    {
        UpdateField();
        PlayerInfoHolder.CoinsAmtUpdated += UpdateField;
    }

    private void OnDisable()
    {
        PlayerInfoHolder.CoinsAmtUpdated -= UpdateField;
    }

    private void UpdateField()
    {
        _countAnim.StartAnimation(_amtField, PlayerInfoHolder.Coins);
    }
}
