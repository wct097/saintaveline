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

    public AudioSource AudioSourcePrefab;
    public AudioClip FireSound;
    public AudioClip ReloadSound;
    public float AudioRange;
}
