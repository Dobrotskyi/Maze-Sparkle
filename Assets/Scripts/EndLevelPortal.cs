using System;
using UnityEngine;

public class EndLevelPortal : Door
{
    public static event Action LevelFinished;

    [SerializeField] private ParticleSystem _levelPassed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Ball player) && IsOpened)
        {
            _levelPassed.gameObject.SetActive(true);
            LevelFinished?.Invoke();
            Destroy(player.gameObject);
        }
    }

    private void Start()
    {
        if (_buttonsToOpen == 0)
            Open();
    }
}
