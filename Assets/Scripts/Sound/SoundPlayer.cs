using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    //Music track: Donut by Lukrembo
    //Source: https://freetouse.com/music
    //Music for Videos(Free Download)
    [SerializeField] private AudioSource _musicAS;
    [SerializeField] private AudioSource _soundAS;
    [SerializeField] private float _musicVolume = 0.5f;

    private void Awake()
    {
        SoundSettings.OnSettingsChanged += OnAudioSettingsChanged;
    }

    private void OnDestroy()
    {
        SoundSettings.OnSettingsChanged -= OnAudioSettingsChanged;
    }

    private void OnAudioSettingsChanged()
    {
        if (_musicAS.volume != _musicVolume && !SoundSettings.MusicMuted)
            _musicAS.volume = _musicVolume;
        else if (_musicAS.volume != 0 && SoundSettings.MusicMuted)
            _musicAS.volume = 0;
    }
}
