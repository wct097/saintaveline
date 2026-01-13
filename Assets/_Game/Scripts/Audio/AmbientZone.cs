using UnityEngine;

/// <summary>
/// Trigger-based ambient sound zone.
/// Attach to a GameObject with a trigger collider.
/// When the player enters, starts playing ambient audio.
/// Supports fade in/out and volume stacking with other zones.
/// </summary>
[RequireComponent(typeof(Collider))]
public class AmbientZone : MonoBehaviour
{
    [Header("Ambient Sound")]
    [SerializeField] private AudioClip _ambientClip;
    [SerializeField] [Range(0f, 1f)] private float _volume = 0.5f;
    [SerializeField] private bool _loop = true;

    [Header("Fade Settings")]
    [SerializeField] private float _fadeInDuration = 1f;
    [SerializeField] private float _fadeOutDuration = 1f;

    [Header("Filtering")]
    [SerializeField] private string _playerTag = "Player";

    private AudioSource _audioSource;
    private Coroutine _fadeCoroutine;
    private bool _isPlayerInside;

    private void Awake()
    {
        // Ensure collider is a trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;

        // Create dedicated audio source for this zone
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = _ambientClip;
        _audioSource.loop = _loop;
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 0f;
        _audioSource.volume = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_playerTag)) return;

        _isPlayerInside = true;
        StartAmbient();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(_playerTag)) return;

        _isPlayerInside = false;
        StopAmbient();
    }

    private void OnDisable()
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }

        if (_audioSource != null && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }

    private void StartAmbient()
    {
        if (_ambientClip == null) return;

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        if (!_audioSource.isPlaying)
        {
            _audioSource.clip = _ambientClip;
            _audioSource.Play();
        }

        _fadeCoroutine = StartCoroutine(FadeVolume(_volume, _fadeInDuration));
    }

    private void StopAmbient()
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        _fadeCoroutine = StartCoroutine(FadeVolume(0f, _fadeOutDuration));
    }

    private System.Collections.IEnumerator FadeVolume(float target, float duration)
    {
        float start = _audioSource.volume;
        float time = 0f;

        // Get volume multiplier from AudioManager if available
        float masterMultiplier = 1f;
        if (AudioManager.Instance != null)
        {
            masterMultiplier = AudioManager.Instance.MasterVolume * AudioManager.Instance.SFXVolume;
        }

        float adjustedTarget = target * masterMultiplier;

        if (duration <= 0f)
        {
            _audioSource.volume = adjustedTarget;
            if (adjustedTarget <= 0f && !_isPlayerInside)
            {
                _audioSource.Stop();
            }
            _fadeCoroutine = null;
            yield break;
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(start, adjustedTarget, time / duration);
            yield return null;
        }

        _audioSource.volume = adjustedTarget;

        if (adjustedTarget <= 0f && !_isPlayerInside)
        {
            _audioSource.Stop();
        }

        _fadeCoroutine = null;
    }

    /// <summary>
    /// Force update the ambient volume (call when global volume settings change).
    /// </summary>
    public void UpdateVolume()
    {
        if (!_isPlayerInside || !_audioSource.isPlaying) return;

        float masterMultiplier = 1f;
        if (AudioManager.Instance != null)
        {
            masterMultiplier = AudioManager.Instance.MasterVolume * AudioManager.Instance.SFXVolume;
        }

        _audioSource.volume = _volume * masterMultiplier;
    }

    /// <summary>
    /// Set a new ambient clip at runtime.
    /// </summary>
    public void SetAmbientClip(AudioClip clip)
    {
        _ambientClip = clip;

        if (_isPlayerInside)
        {
            _audioSource.clip = clip;
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
    }
}
