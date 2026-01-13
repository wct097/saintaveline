using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central audio manager singleton with audio source pooling.
/// Handles SFX playback with 3D positioning and music playback.
/// </summary>
[DefaultExecutionOrder(-90)]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Pool Settings")]
    [SerializeField] private int _poolSize = 16;
    [SerializeField] private GameObject _audioSourcePrefab;

    [Header("Volume Settings")]
    [SerializeField] [Range(0f, 1f)] private float _masterVolume = 1f;
    [SerializeField] [Range(0f, 1f)] private float _sfxVolume = 1f;
    [SerializeField] [Range(0f, 1f)] private float _musicVolume = 1f;

    private List<AudioSource> _sfxPool;
    private int _poolIndex;
    private AudioSource _musicSource;

    public float MasterVolume
    {
        get => _masterVolume;
        set
        {
            _masterVolume = Mathf.Clamp01(value);
            UpdateMusicVolume();
        }
    }

    public float SFXVolume
    {
        get => _sfxVolume;
        set => _sfxVolume = Mathf.Clamp01(value);
    }

    public float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = Mathf.Clamp01(value);
            UpdateMusicVolume();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializePool();
        InitializeMusicSource();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void InitializePool()
    {
        _sfxPool = new List<AudioSource>(_poolSize);

        for (int i = 0; i < _poolSize; i++)
        {
            AudioSource source = CreatePooledAudioSource();
            _sfxPool.Add(source);
        }
    }

    private AudioSource CreatePooledAudioSource()
    {
        GameObject obj;

        if (_audioSourcePrefab != null)
        {
            obj = Instantiate(_audioSourcePrefab, transform);
        }
        else
        {
            obj = new GameObject("PooledAudioSource");
            obj.transform.SetParent(transform);
        }

        AudioSource source = obj.GetComponent<AudioSource>();
        if (source == null)
        {
            source = obj.AddComponent<AudioSource>();
        }

        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.minDistance = 1f;
        source.maxDistance = 30f;
        obj.SetActive(false);

        return source;
    }

    private void InitializeMusicSource()
    {
        GameObject musicObj = new GameObject("MusicSource");
        musicObj.transform.SetParent(transform);

        _musicSource = musicObj.AddComponent<AudioSource>();
        _musicSource.playOnAwake = false;
        _musicSource.loop = true;
        _musicSource.spatialBlend = 0f;
        _musicSource.volume = _masterVolume * _musicVolume;
    }

    private void UpdateMusicVolume()
    {
        if (_musicSource != null)
        {
            _musicSource.volume = _masterVolume * _musicVolume;
        }
    }

    private AudioSource GetAvailableSource()
    {
        // Try to find an inactive source
        for (int i = 0; i < _sfxPool.Count; i++)
        {
            int index = (_poolIndex + i) % _sfxPool.Count;
            if (!_sfxPool[index].isPlaying)
            {
                _poolIndex = (index + 1) % _sfxPool.Count;
                return _sfxPool[index];
            }
        }

        // All sources busy, expand pool
        AudioSource newSource = CreatePooledAudioSource();
        _sfxPool.Add(newSource);
        return newSource;
    }

    /// <summary>
    /// Play a sound effect at a specific world position.
    /// </summary>
    public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableSource();
        source.gameObject.SetActive(true);
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume * _sfxVolume * _masterVolume;
        source.pitch = pitch;
        source.spatialBlend = 1f;
        source.Play();

        StartCoroutine(ReturnToPoolWhenDone(source, clip.length / pitch));
    }

    /// <summary>
    /// Play a 2D sound effect (no spatial positioning).
    /// </summary>
    public void PlaySFX2D(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableSource();
        source.gameObject.SetActive(true);
        source.clip = clip;
        source.volume = volume * _sfxVolume * _masterVolume;
        source.pitch = pitch;
        source.spatialBlend = 0f;
        source.Play();

        StartCoroutine(ReturnToPoolWhenDone(source, clip.length / pitch));
    }

    /// <summary>
    /// Play a random clip from an array at a specific position.
    /// </summary>
    public void PlayRandomSFX(AudioClip[] clips, Vector3 position, float volume = 1f, float pitchVariation = 0.1f)
    {
        if (clips == null || clips.Length == 0) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        float pitch = Random.Range(1f - pitchVariation, 1f + pitchVariation);
        PlaySFX(clip, position, volume, pitch);
    }

    /// <summary>
    /// Play a random clip from an array as 2D sound.
    /// </summary>
    public void PlayRandomSFX2D(AudioClip[] clips, float volume = 1f, float pitchVariation = 0.1f)
    {
        if (clips == null || clips.Length == 0) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        float pitch = Random.Range(1f - pitchVariation, 1f + pitchVariation);
        PlaySFX2D(clip, volume, pitch);
    }

    private System.Collections.IEnumerator ReturnToPoolWhenDone(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration + 0.1f);

        if (source != null)
        {
            source.Stop();
            source.clip = null;
            source.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Play background music with optional crossfade.
    /// </summary>
    public void PlayMusic(AudioClip clip, float fadeDuration = 1f)
    {
        if (clip == null) return;

        StartCoroutine(CrossfadeMusic(clip, fadeDuration));
    }

    /// <summary>
    /// Stop background music with fade out.
    /// </summary>
    public void StopMusic(float fadeDuration = 1f)
    {
        StartCoroutine(FadeOutMusic(fadeDuration));
    }

    private System.Collections.IEnumerator CrossfadeMusic(AudioClip newClip, float duration)
    {
        float targetVolume = _masterVolume * _musicVolume;

        // Fade out current music if playing
        if (_musicSource.isPlaying)
        {
            float startVolume = _musicSource.volume;
            float time = 0f;

            while (time < duration / 2f)
            {
                time += Time.unscaledDeltaTime;
                _musicSource.volume = Mathf.Lerp(startVolume, 0f, time / (duration / 2f));
                yield return null;
            }

            _musicSource.Stop();
        }

        // Start new music
        _musicSource.clip = newClip;
        _musicSource.volume = 0f;
        _musicSource.Play();

        // Fade in
        float fadeTime = 0f;
        while (fadeTime < duration / 2f)
        {
            fadeTime += Time.unscaledDeltaTime;
            _musicSource.volume = Mathf.Lerp(0f, targetVolume, fadeTime / (duration / 2f));
            yield return null;
        }

        _musicSource.volume = targetVolume;
    }

    private System.Collections.IEnumerator FadeOutMusic(float duration)
    {
        if (!_musicSource.isPlaying) yield break;

        float startVolume = _musicSource.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            _musicSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        _musicSource.Stop();
        _musicSource.volume = 0f;
    }

    /// <summary>
    /// Check if music is currently playing.
    /// </summary>
    public bool IsMusicPlaying => _musicSource != null && _musicSource.isPlaying;

    /// <summary>
    /// Get the current music clip.
    /// </summary>
    public AudioClip CurrentMusicClip => _musicSource?.clip;
}
