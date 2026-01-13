using UnityEngine;

/// <summary>
/// Initializes game state at runtime. Attach to any active GameObject in the Game scene.
/// Fixes:
/// 1. Enables the "== ENEMY NPCS ==" container which is disabled by default
/// 2. Auto-equips the player with a weapon if they don't have one
/// </summary>
public class GameInitializer : MonoBehaviour
{
    [Header("Auto-Enable Objects")]
    [SerializeField] private string _enemyContainerName = "== ENEMY NPCS ==";

    [Header("Player Weapon")]
    [SerializeField] private bool _autoEquipWeapon = true;
    [SerializeField] private string _defaultWeaponName = "PF_Pistol1";

    void Start()
    {
        EnableEnemies();

        if (_autoEquipWeapon)
        {
            AutoEquipPlayerWeapon();
        }
    }

    void EnableEnemies()
    {
        var enemyContainer = GameObject.Find(_enemyContainerName);
        if (enemyContainer == null)
        {
            // Try to find inactive object
            var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var obj in allObjects)
            {
                if (obj.name == _enemyContainerName && obj.scene.IsValid())
                {
                    enemyContainer = obj;
                    break;
                }
            }
        }

        if (enemyContainer != null)
        {
            enemyContainer.SetActive(true);
            Debug.Log($"GameInitializer: Enabled '{_enemyContainerName}'");
        }
        else
        {
            Debug.LogWarning($"GameInitializer: Could not find '{_enemyContainerName}' to enable");
        }
    }

    void AutoEquipPlayerWeapon()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("GameInitializer: Player not found");
            return;
        }

        var playerEntity = player.GetComponent<CharacterEntity>();
        if (playerEntity == null)
        {
            Debug.LogWarning("GameInitializer: Player has no CharacterEntity");
            return;
        }

        // Player already has a weapon equipped
        if (playerEntity.EquippedItem2 != null)
        {
            Debug.Log("GameInitializer: Player already has weapon equipped");
            return;
        }

        // Find weapon in scene
        var weapon = GameObject.Find(_defaultWeaponName);
        if (weapon == null)
        {
            // Try finding inactive
            var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var obj in allObjects)
            {
                if (obj.name == _defaultWeaponName && obj.scene.IsValid())
                {
                    weapon = obj;
                    break;
                }
            }
        }

        if (weapon != null)
        {
            var itemEntity = weapon.GetComponent<ItemEntity>();
            if (itemEntity != null)
            {
                playerEntity.SetEquippedItem(itemEntity);
                Debug.Log($"GameInitializer: Auto-equipped player with '{_defaultWeaponName}'");
            }
            else
            {
                Debug.LogWarning($"GameInitializer: '{_defaultWeaponName}' has no ItemEntity component");
            }
        }
        else
        {
            Debug.LogWarning($"GameInitializer: Could not find weapon '{_defaultWeaponName}'");
        }
    }
}
