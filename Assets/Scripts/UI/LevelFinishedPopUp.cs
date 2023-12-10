using TMPro;
using UnityEngine;

public class LevelFinishedPopUp : MonoBehaviour
{
    [SerializeField] private GameObject _body;
    [SerializeField] private TextMeshProUGUI _coinsRewardField;

    private void Awake()
    {
        EndLevelPortal.LevelFinished += OnGameOver;
    }

    private void OnDestroy()
    {
        EndLevelPortal.LevelFinished -= OnGameOver;
    }

    private void OnGameOver()
    {
        _body.SetActive(true);
        _coinsRewardField.text = "+" + PlayerInfoHolder.REWARD;
    }
}
