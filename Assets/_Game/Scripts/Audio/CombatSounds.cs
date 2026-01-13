using UnityEngine;

/// <summary>
/// Helper class for playing combat-related sounds through AudioManager.
/// Provides static methods for gunshots, impacts, enemy alerts, and death sounds.
/// Can be integrated by weapon/combat systems to use pooled audio.
/// </summary>
public class CombatSounds : MonoBehaviour
{
    public static CombatSounds Instance { get; private set; }

    [Header("Weapon Sounds")]
    [SerializeField] private AudioClip[] _gunshotClips;
    [SerializeField] private AudioClip[] _reloadClips;
    [SerializeField] private AudioClip _emptyClickClip;

    [Header("Impact Sounds")]
    [SerializeField] private AudioClip[] _bulletImpactFlesh;
    [SerializeField] private AudioClip[] _bulletImpactMetal;
    [SerializeField] private AudioClip[] _bulletImpactConcrete;
    [SerializeField] private AudioClip[] _bulletImpactWood;

    [Header("Enemy Sounds")]
    [SerializeField] private AudioClip[] _enemyAlertClips;
    [SerializeField] private AudioClip[] _enemyDeathClips;
    [SerializeField] private AudioClip[] _enemyHurtClips;

    [Header("Player Sounds")]
    [SerializeField] private AudioClip[] _playerHurtClips;
    [SerializeField] private AudioClip _playerDeathClip;

    [Header("Volume Settings")]
    [SerializeField] [Range(0f, 1f)] private float _weaponVolume = 0.8f;
    [SerializeField] [Range(0f, 1f)] private float _impactVolume = 0.6f;
    [SerializeField] [Range(0f, 1f)] private float _voiceVolume = 0.7f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    /// <summary>
    /// Play a gunshot sound at the specified position.
    /// </summary>
    public void PlayGunshot(Vector3 position, float volumeMultiplier = 1f)
    {
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.PlayRandomSFX(_gunshotClips, position, _weaponVolume * volumeMultiplier);
    }

    /// <summary>
    /// Play a reload sound at the specified position.
    /// </summary>
    public void PlayReload(Vector3 position)
    {
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.PlayRandomSFX(_reloadClips, position, _weaponVolume);
    }

    /// <summary>
    /// Play an empty click sound (out of ammo).
    /// </summary>
    public void PlayEmptyClick(Vector3 position)
    {
        if (AudioManager.Instance == null || _emptyClickClip == null) return;

        AudioManager.Instance.PlaySFX(_emptyClickClip, position, _weaponVolume);
    }

    /// <summary>
    /// Play a bullet impact sound based on surface type.
    /// </summary>
    public void PlayBulletImpact(Vector3 position, SurfaceType surfaceType)
    {
        if (AudioManager.Instance == null) return;

        AudioClip[] clips = surfaceType switch
        {
            SurfaceType.Wood => _bulletImpactWood,
            SurfaceType.Dirt => _bulletImpactConcrete, // Dirt uses concrete fallback
            _ => _bulletImpactConcrete
        };

        if (clips == null || clips.Length == 0)
        {
            clips = _bulletImpactConcrete;
        }

        AudioManager.Instance.PlayRandomSFX(clips, position, _impactVolume);
    }

    /// <summary>
    /// Play a bullet impact sound on flesh (enemy/player hit).
    /// </summary>
    public void PlayFleshImpact(Vector3 position)
    {
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.PlayRandomSFX(_bulletImpactFlesh, position, _impactVolume);
    }

    /// <summary>
    /// Play a bullet impact sound on metal.
    /// </summary>
    public void PlayMetalImpact(Vector3 position)
    {
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.PlayRandomSFX(_bulletImpactMetal, position, _impactVolume);
    }

    /// <summary>
    /// Play enemy alert sound (spotted player).
    /// </summary>
    public void PlayEnemyAlert(Vector3 position)
    {
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.PlayRandomSFX(_enemyAlertClips, position, _voiceVolume);
    }

    /// <summary>
    /// Play enemy death sound.
    /// </summary>
    public void PlayEnemyDeath(Vector3 position)
    {
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.PlayRandomSFX(_enemyDeathClips, position, _voiceVolume);
    }

    /// <summary>
    /// Play enemy hurt sound.
    /// </summary>
    public void PlayEnemyHurt(Vector3 position)
    {
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.PlayRandomSFX(_enemyHurtClips, position, _voiceVolume);
    }

    /// <summary>
    /// Play player hurt sound (2D, not positional).
    /// </summary>
    public void PlayPlayerHurt()
    {
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.PlayRandomSFX2D(_playerHurtClips, _voiceVolume);
    }

    /// <summary>
    /// Play player death sound (2D, not positional).
    /// </summary>
    public void PlayPlayerDeath()
    {
        if (AudioManager.Instance == null || _playerDeathClip == null) return;

        AudioManager.Instance.PlaySFX2D(_playerDeathClip, _voiceVolume);
    }

    // Static convenience methods
    public static void Gunshot(Vector3 position, float volumeMultiplier = 1f)
    {
        if (Instance != null) Instance.PlayGunshot(position, volumeMultiplier);
    }

    public static void Reload(Vector3 position)
    {
        if (Instance != null) Instance.PlayReload(position);
    }

    public static void EmptyClick(Vector3 position)
    {
        if (Instance != null) Instance.PlayEmptyClick(position);
    }

    public static void BulletImpact(Vector3 position, SurfaceType surfaceType)
    {
        if (Instance != null) Instance.PlayBulletImpact(position, surfaceType);
    }

    public static void FleshImpact(Vector3 position)
    {
        if (Instance != null) Instance.PlayFleshImpact(position);
    }

    public static void MetalImpact(Vector3 position)
    {
        if (Instance != null) Instance.PlayMetalImpact(position);
    }

    public static void EnemyAlert(Vector3 position)
    {
        if (Instance != null) Instance.PlayEnemyAlert(position);
    }

    public static void EnemyDeath(Vector3 position)
    {
        if (Instance != null) Instance.PlayEnemyDeath(position);
    }

    public static void EnemyHurt(Vector3 position)
    {
        if (Instance != null) Instance.PlayEnemyHurt(position);
    }

    public static void PlayerHurt()
    {
        if (Instance != null) Instance.PlayPlayerHurt();
    }

    public static void PlayerDeath()
    {
        if (Instance != null) Instance.PlayPlayerDeath();
    }
}
