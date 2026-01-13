using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dynamic music system that crossfades between calm and combat music
/// based on enemy proximity to the player.
/// </summary>
public class TensionMusic : MonoBehaviour
{
    public static TensionMusic Instance { get; private set; }

    [Header("Music Tracks")]
    [SerializeField] private AudioClip _calmMusic;
    [SerializeField] private AudioClip _combatMusic;

    [Header("Volume Settings")]
    [SerializeField] [Range(0f, 1f)] private float _calmVolume = 0.4f;
    [SerializeField] [Range(0f, 1f)] private float _combatVolume = 0.6f;

    [Header("Detection Settings")]
    [SerializeField] private float _combatTriggerDistance = 15f;
    [SerializeField] private float _combatReleaseDistance = 25f;
    [SerializeField] private float _checkInterval = 0.5f;

    [Header("Crossfade Settings")]
    [SerializeField] private float _crossfadeDuration = 2f;
    [SerializeField] private float _combatCooldown = 5f;

    private AudioSource _calmSource;
    private AudioSource _combatSource;
    private Transform _playerTransform;
    private List<EnemyNPC> _cachedEnemies;
    private float _checkTimer;
    private float _lastCombatTime;
    private bool _inCombatMode;
    private Coroutine _crossfadeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializeAudioSources();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        // Find player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }

        // Cache enemies initially
        RefreshEnemyCache();

        // Start calm music
        if (_calmMusic != null)
        {
            _calmSource.clip = _calmMusic;
            _calmSource.Play();
            _calmSource.volume = GetAdjustedVolume(_calmVolume);
        }

        if (_combatMusic != null)
        {
            _combatSource.clip = _combatMusic;
            _combatSource.Play();
            _combatSource.volume = 0f;
        }
    }

    private void InitializeAudioSources()
    {
        // Create calm music source
        GameObject calmObj = new GameObject("CalmMusicSource");
        calmObj.transform.SetParent(transform);
        _calmSource = calmObj.AddComponent<AudioSource>();
        _calmSource.playOnAwake = false;
        _calmSource.loop = true;
        _calmSource.spatialBlend = 0f;
        _calmSource.volume = 0f;

        // Create combat music source
        GameObject combatObj = new GameObject("CombatMusicSource");
        combatObj.transform.SetParent(transform);
        _combatSource = combatObj.AddComponent<AudioSource>();
        _combatSource.playOnAwake = false;
        _combatSource.loop = true;
        _combatSource.spatialBlend = 0f;
        _combatSource.volume = 0f;
    }

    private void Update()
    {
        if (_playerTransform == null) return;

        _checkTimer -= Time.deltaTime;
        if (_checkTimer <= 0f)
        {
            _checkTimer = _checkInterval;
            CheckEnemyProximity();
        }
    }

    private void CheckEnemyProximity()
    {
        if (_cachedEnemies == null || _cachedEnemies.Count == 0)
        {
            RefreshEnemyCache();
        }

        bool enemyNearby = false;
        float closestDistance = float.MaxValue;

        // Clean up destroyed enemies and check distances
        for (int i = _cachedEnemies.Count - 1; i >= 0; i--)
        {
            if (_cachedEnemies[i] == null)
            {
                _cachedEnemies.RemoveAt(i);
                continue;
            }

            float distance = Vector3.Distance(_playerTransform.position, _cachedEnemies[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
            }
        }

        // Determine if we should be in combat mode using hysteresis
        if (_inCombatMode)
        {
            // Use larger release distance to prevent rapid switching
            enemyNearby = closestDistance < _combatReleaseDistance;
        }
        else
        {
            // Use smaller trigger distance to enter combat
            enemyNearby = closestDistance < _combatTriggerDistance;
        }

        // Apply combat cooldown
        if (enemyNearby)
        {
            _lastCombatTime = Time.time;
        }

        bool shouldBeCombat = enemyNearby || (Time.time - _lastCombatTime < _combatCooldown);

        if (shouldBeCombat && !_inCombatMode)
        {
            TransitionToCombat();
        }
        else if (!shouldBeCombat && _inCombatMode)
        {
            TransitionToCalm();
        }
    }

    private void TransitionToCombat()
    {
        _inCombatMode = true;

        if (_crossfadeRoutine != null)
        {
            StopCoroutine(_crossfadeRoutine);
        }

        _crossfadeRoutine = StartCoroutine(CrossfadeRoutine(_combatSource, _calmSource, _combatVolume, _crossfadeDuration));
    }

    private void TransitionToCalm()
    {
        _inCombatMode = false;

        if (_crossfadeRoutine != null)
        {
            StopCoroutine(_crossfadeRoutine);
        }

        _crossfadeRoutine = StartCoroutine(CrossfadeRoutine(_calmSource, _combatSource, _calmVolume, _crossfadeDuration));
    }

    private System.Collections.IEnumerator CrossfadeRoutine(AudioSource fadeIn, AudioSource fadeOut, float targetVolume, float duration)
    {
        float fadeInStart = fadeIn.volume;
        float fadeOutStart = fadeOut.volume;
        float adjustedTarget = GetAdjustedVolume(targetVolume);
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // Smooth crossfade curve
            float smoothT = t * t * (3f - 2f * t);

            fadeIn.volume = Mathf.Lerp(fadeInStart, adjustedTarget, smoothT);
            fadeOut.volume = Mathf.Lerp(fadeOutStart, 0f, smoothT);

            yield return null;
        }

        fadeIn.volume = adjustedTarget;
        fadeOut.volume = 0f;
        _crossfadeRoutine = null;
    }

    private float GetAdjustedVolume(float baseVolume)
    {
        if (AudioManager.Instance != null)
        {
            return baseVolume * AudioManager.Instance.MasterVolume * AudioManager.Instance.MusicVolume;
        }
        return baseVolume;
    }

    /// <summary>
    /// Refresh the cached list of enemies.
    /// Call this when enemies are spawned or destroyed.
    /// </summary>
    public void RefreshEnemyCache()
    {
        EnemyNPC[] enemies = FindObjectsOfType<EnemyNPC>();
        _cachedEnemies = new List<EnemyNPC>(enemies);
    }

    /// <summary>
    /// Force transition to combat music.
    /// </summary>
    public void ForceCombatMode()
    {
        _lastCombatTime = Time.time;
        if (!_inCombatMode)
        {
            TransitionToCombat();
        }
    }

    /// <summary>
    /// Update volume when global audio settings change.
    /// </summary>
    public void UpdateVolume()
    {
        if (_inCombatMode)
        {
            _combatSource.volume = GetAdjustedVolume(_combatVolume);
            _calmSource.volume = 0f;
        }
        else
        {
            _calmSource.volume = GetAdjustedVolume(_calmVolume);
            _combatSource.volume = 0f;
        }
    }

    /// <summary>
    /// Stop all music.
    /// </summary>
    public void StopMusic()
    {
        if (_crossfadeRoutine != null)
        {
            StopCoroutine(_crossfadeRoutine);
            _crossfadeRoutine = null;
        }

        _calmSource.Stop();
        _combatSource.Stop();
    }

    /// <summary>
    /// Resume music playback.
    /// </summary>
    public void ResumeMusic()
    {
        if (_calmMusic != null && !_calmSource.isPlaying)
        {
            _calmSource.Play();
        }
        if (_combatMusic != null && !_combatSource.isPlaying)
        {
            _combatSource.Play();
        }

        UpdateVolume();
    }
}
