using System;
using UnityEngine;

public class EndLevelPortal : Door
{
    public static event Action LevelFinished;

    [SerializeField] private ParticleSystem _levelPassed;
    private AudioSource _as;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Ball player) && IsOpened)
        {
            _levelPassed.gameObject.SetActive(true);
            LevelFinished?.Invoke();
            if (!SoundSettings.AudioMuted)
                _as.Play();
            Destroy(player.gameObject);
        }
    }

    private void Start()
    {
        _as = GetComponent<AudioSource>();
        _as.volume = SoundSettings.EFFECTS_VOLUME;
        if (_buttonsToOpen == 0)
            Open();
    }
}
