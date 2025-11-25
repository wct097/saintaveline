#nullable enable

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public string ItemName = null!;
    public float DamageScore;

    public GameObject prefab = null!;

    public LayerMask TargetCollisionLayers;
    public Vector3 EquippedPosition = new Vector3(0, 0, 0);
    public Vector3 EquippedRotation = new Vector3(0, 0, 0);

    // TODO: is this really needed?
    public List<InteractionData> Interactions = null!;

    public bool Equippable;
    public bool Storable;

    public Sprite? Thumbnail = null;
}
