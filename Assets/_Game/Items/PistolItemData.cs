using UnityEngine;

[CreateAssetMenu(fileName = "NewPistolItem", menuName = "Game/PistolItem")]
public class PistolItemData : ItemData
{
    public float RecoilDuration;
    public float HoldDuration;
    public float ReturnDuration;
    
    public float FireRange;
    public Vector3 FirePoint;

    // Ammo system
    public int MagazineSize = 12;
    public int StartingAmmo = 12;
    public float ReloadTime = 2f;

    // Distance falloff - damage multiplier decreases linearly from 1.0 at FalloffStartRange to MinDamageMultiplier at FireRange
    public float FalloffStartRange = 10f;
    public float MinDamageMultiplier = 0.3f;

    public AudioSource AudioSourcePrefab;
    public AudioClip FireSound;
    public AudioClip ReloadSound;
    public AudioClip HitSound;
    public float AudioRange;
}
