using UnityEngine;

public class BounceSoundPlayer : MonoBehaviour
{
    private AudioSource _as;

    private void Awake()
    {
        _as = GetComponent<AudioSource>();
        _as.volume = SoundSettings.EFFECTS_VOLUME;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_as.enabled && !SoundSettings.AudioMuted)
            _as.Play();
    }
}
