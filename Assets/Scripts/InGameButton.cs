using UnityEngine;
using System.Collections.Generic;

public class InGameButton : MonoBehaviour
{
    [SerializeField] private GameObject _statusOn;
    [SerializeField] private GameObject _statusOff;
    [SerializeField] private List<Door> _doors;

    private void Awake()
    {
        _statusOn.SetActive(false);
        _statusOff.SetActive(true);
        foreach (var door in _doors)
            door.AddToOpenButtons();
    }

    private void OnDisable()
    {
        OpenConnectedDoors();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("ShadowBall")) && _statusOff.activeSelf)
        {
            OpenConnectedDoors();
            _statusOff.SetActive(false);
            _statusOn.SetActive(true);
        }
    }

    private void OpenConnectedDoors()
    {
        foreach (Door door in _doors)
            door.ButtonPressed();
    }
}
