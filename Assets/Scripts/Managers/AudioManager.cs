using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SFXClipEntry
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSourceA;
    [SerializeField] private AudioSource _musicSourceB;
    [SerializeField] private AudioSource _sfxSource;

    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float _musicVolume = 0.4f;
    [SerializeField] private float _fadeDuration = 2f;

    [Header("Music Tracks")]
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameMusic;

    [Header("Audio Clips")]
    public SFXClipEntry[] sfxClips;

    private AudioSource _activeMusicSource;
    private bool _musicMuted = false;
    private bool _sfxMuted = false;
    private Dictionary<string, AudioClip> _sfxByName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        BuildSFXDictionary();
    }

    private void BuildSFXDictionary()
    {
        _sfxByName = new Dictionary<string, AudioClip>(StringComparer.OrdinalIgnoreCase);

        if (sfxClips == null)
            return;

        foreach (var entry in sfxClips)
        {
            if (entry == null || entry.clip == null || string.IsNullOrWhiteSpace(entry.name))
                continue;

            _sfxByName[entry.name] = entry.clip;
        }
    }

    private void Start()
    {
        PlayMusic(_menuMusic, true);
    }

    public void PlayMusic(AudioClip newTrack, bool instant = false)
    {
        if (newTrack == null) return;

        AudioSource inactiveSource = (_activeMusicSource == _musicSourceA) ? _musicSourceB : _musicSourceA;

        inactiveSource.clip = newTrack;
        inactiveSource.loop = true;
        inactiveSource.volume = instant ? _musicVolume : 0f;
        inactiveSource.Play();

        if (instant)
        {
            if (_activeMusicSource != null) _activeMusicSource.Stop();
            _activeMusicSource = inactiveSource;
        }
        else
        {
            StartCoroutine(FadeMusic(inactiveSource));
        }
    }

    private IEnumerator FadeMusic(AudioSource newSource)
    {
        AudioSource oldSource = _activeMusicSource;
        _activeMusicSource = newSource;

        float time = 0f;
        while (time < _fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / _fadeDuration;

            if (oldSource != null)
                oldSource.volume = Mathf.Lerp(_musicVolume, 0f, t);

            newSource.volume = Mathf.Lerp(0f, _musicVolume, t);

            yield return null;
        }

        if (oldSource != null)
        {
            oldSource.Stop();
        }
        newSource.volume = _musicVolume;
    }

    public void MuteMusic(bool mute)
    {
        _musicMuted = mute;
        _musicSourceA.mute = mute;
        _musicSourceB.mute = mute;
    }


    public void PlaySFX(string clipName)
    {
        if (_sfxSource == null || _sfxMuted) return;
        if (_sfxByName == null) BuildSFXDictionary();

        if (_sfxByName != null && _sfxByName.TryGetValue(clipName, out AudioClip clip) && clip != null)
        {
            _sfxSource.PlayOneShot(clip);
            return;
        }

        Debug.LogWarning($"No SFX clip found with name '{clipName}'.");
    }

    public void PlaySFX(int clipIndex)
    {
        if (_sfxSource == null || sfxClips == null) return;
        if (clipIndex < 0 || clipIndex >= sfxClips.Length) return;
        if (_sfxMuted) return;

        var entry = sfxClips[clipIndex];
        if (entry != null && entry.clip != null)
        {
            _sfxSource.PlayOneShot(entry.clip);
        }
    }

    public void MuteSFX(bool mute)
    {
        _sfxMuted = mute;
        if (_sfxSource != null)
        {
            _sfxSource.mute = mute;
        }
    }

    public bool IsMusicMuted() => _musicMuted;
    public bool IsSFXMuted() => _sfxMuted;

    public void PlayMenuMusic() => PlayMusic(_menuMusic);
    public void PlayGameMusic() => PlayMusic(_gameMusic);
}
